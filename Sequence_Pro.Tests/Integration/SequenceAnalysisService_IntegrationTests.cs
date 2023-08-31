using Dapper;
using FluentValidation;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Validators;
using Sequence_Pro.Tests.TestDatabase;
using Sequence_Pro.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.Integration;
public class SequenceAnalysisService_IntegrationTests : IAsyncLifetime
{
    private readonly string _testDbConnectionString = "Server=localhost;Port=5433;Database=testdb;User ID=user;Password=changeme";
    private readonly ISequenceAnalysisRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IUniprotAPI _uniprotAPI;
    private readonly ISequenceAnalyser _sequenceAnalyser;
    private readonly IValidator<string> _requestValidator;
    private readonly TestDbManager _testDbManager;

    private readonly ISequenceAnalysisService _sut;

    public SequenceAnalysisService_IntegrationTests()
    {
        //set up system under test
        _requestValidator = new RequestValidator();
        _httpClient = new HttpClient();
        _dbConnectionFactory = new NpgsqlConnectionFactory(_testDbConnectionString);
        _repository = new SequenceAnalysisRepository(_dbConnectionFactory);
        _uniprotAPI = new UniprotAPI();
        _sequenceAnalyser = new SequenceAnalyser();
        _repository = new SequenceAnalysisRepository(_dbConnectionFactory);

        _sut = new SequenceAnalysisService(_repository, _httpClient, _uniprotAPI, _sequenceAnalyser, _requestValidator);

        //initialise test db
        _testDbManager = new TestDbManager(_dbConnectionFactory);
    }

    public async Task InitializeAsync() => await _testDbManager.InitialiseAsync();


    public async Task DisposeAsync() => await _testDbManager.ClearTestDbAsync();

    [Fact]
    public async Task SequenceAnalysisService_CreateAsync_Posts_To_Database()
    {
        //Arrange
        using var validationConnection = await _dbConnectionFactory.CreateConnectionAsync();

        //Act
        var sequenceAnalysis = await _sut.CreateAsync("P12345");

        //Assert
        var result = await validationConnection.ExecuteScalarAsync<bool>("""
            select case when exists(
            select 1 from Sequences where id = @Id)
            then 1 else 0 end;
            """, new { sequenceAnalysis.Id });

        Assert.True(result);
        Assert.IsType<SequenceAnalysis>(sequenceAnalysis);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByIdAsync_Returns_Null_When_No_Matching_Record_Found()
    {
        //Arrange
        var sequenceAnalysis = SequenceAnalysisExample.Create();
        var id = sequenceAnalysis.Id;

        //Act
        var result = await _sut.GetByIdAsync(id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByIdAsync_Returns_SequenceAnalysis_When_Matching_Record_Found()
    {
        //Arrange
        var id = await _testDbManager.PostTestRecord();
        
        //Act
        var result = await _sut.GetByIdAsync(id);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByUniprotIdAsync_Returns_Null_When_No_Matching_Record_Found()
    {
        //Arrange
        var uniprotId = "P12341";

        //Act
        var result = await _sut.GetByUniprotIdAsync(uniprotId);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByUniprotIdAsync_Returns_SequenceAnalysis_When_Matching_Record_Found()
    {
        //Arrange
        var uniprotId = "P12563";

        //Act
        var result = await _sut.GetByUniprotIdAsync(uniprotId);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
        Assert.Equal(uniprotId, result.UniprotId);
    }






}

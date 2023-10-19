using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Database.Mapping;
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
public class SequenceAnalysisService_IntegrationTests
{
    private readonly string _testDbConnectionString = "Server=localhost;Port=5433;Database=testdb;User ID=user;Password=changeme";
    private readonly SequenceProContext _testDbContext;
    private readonly ISequenceAnalysisRepository _repository;
    private readonly HttpClient _httpClient;
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
        _uniprotAPI = new UniprotAPI();
        _sequenceAnalyser = new SequenceAnalyser();
        _testDbContext = new SequenceProContext(new DbContextOptionsBuilder<SequenceProContext>()
            .UseNpgsql(_testDbConnectionString)
            .Options);
        _repository = new SequenceAnalysisRepository(_testDbContext);
        _testDbManager = new TestDbManager(_testDbConnectionString);

        _sut = new SequenceAnalysisService(_repository, _httpClient, _uniprotAPI, _sequenceAnalyser, _requestValidator);
    }

    [Fact]
    public async Task SequenceAnalysisService_CreateAsync_Posts_To_Database()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();

        //Act
        var sequenceAnalysisSaved = await _sut.CreateAsync("P12345");

        //Assert
        var sequenceAnalysisQueried = await _testDbContext.SequenceAnalyses
            .Include(x => x.AminoAcidCompositions)
            .Where(x => x.Id == sequenceAnalysisSaved.Id)
            .SingleOrDefaultAsync();

        Assert.NotNull(sequenceAnalysisQueried);
        Assert.Equivalent(sequenceAnalysisSaved, sequenceAnalysisQueried.MapToObject()
            , strict: true);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByIdAsync_Returns_Null_When_No_Matching_Record_Found()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();

        //Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByIdAsync_Returns_SequenceAnalysis_When_Matching_Record_Found()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();
        var entity = SequenceAnalysisExample.Create();
        _testDbContext.SequenceAnalyses.Add(entity.MapToEntity());
        await _testDbContext.SaveChangesAsync();
        
        //Act
        var result = await _sut.GetByIdAsync(entity.Id);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
        Assert.Equivalent(entity, result);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetByUniprotIdAsync_Returns_Null_When_No_Matching_Record_Found()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();
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
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();
        var uniprotId = "P12563";

        //Act
        var result = await _sut.GetByUniprotIdAsync(uniprotId);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
        Assert.Equal(uniprotId, result.UniprotId);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetAllAsync_Returns_Empty_Collection_When_No_Records_Found()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();
        await _testDbContext.SequenceAnalyses.ExecuteDeleteAsync();

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SequenceAnalysisService_GetAllAsync_Returns_IEnumerable_SequenceAnalysis_When_Records_Found()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<SequenceAnalysis>>(result);
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task SequenceAnalysisService_DeleteByIdAsync_Removes_Record_With_Matching_Id()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();
        var entity = SequenceAnalysisExample.Create().MapToEntity();
        _testDbContext.SequenceAnalyses.Add(entity);
        await _testDbContext.SaveChangesAsync();

        //Act
        var result = await _sut.DeleteByIdAsync(entity.Id);

        //Assert
        var record = await _testDbContext.SequenceAnalyses
            .Where(x => x.Id == entity.Id)
            .SingleOrDefaultAsync();

        Assert.True(result);
        Assert.Null(record);
    }

    [Fact]
    public async Task SequenceAnalysisService_DeleteByIdAsync_Returns_False_When_No_Matching_Record()
    {
        //Arrange
        await _testDbManager.InitialiseDatabaseWithFiveRecordsAsync();

        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());

        //Assert
        Assert.False(result);
    }
}

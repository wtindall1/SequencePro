using FluentValidation;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Validators;
using Sequence_Pro.Tests.TestDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.Integration;
public class SequenceAnalysisService_IntegrationTests : IAsyncLifetime
{
    private readonly string _testDbConnectionString = "Server=localhost;Port=5432;Database=localdb;User ID=user;Password=changeme";
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

    



}

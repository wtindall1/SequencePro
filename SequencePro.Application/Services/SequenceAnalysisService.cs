using FluentValidation;
using SequencePro.Application.Interfaces;
using SequencePro.Application.Models;
using SequencePro.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Services;
public class SequenceAnalysisService : ISequenceAnalysisService
{
    private readonly ISequenceAnalysisRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly IUniprotAPI _uniprotAPI;
    private readonly ISequenceAnalyser _sequenceAnalyser;
    private readonly IValidator<string> _requestValidator;
    private readonly IValidator<GetAllSequenceAnalysisOptions> _getAllOptionsValidator;

    public SequenceAnalysisService(ISequenceAnalysisRepository repository, 
        HttpClient httpClient,
        IUniprotAPI uniprotAPI,
        ISequenceAnalyser sequenceAnalyser,
        IValidator<string> requestValidator,
        IValidator<GetAllSequenceAnalysisOptions> getAllOptionsValidator)
    {
        _repository = repository;
        _httpClient = httpClient;
        _uniprotAPI = uniprotAPI;
        _sequenceAnalyser = sequenceAnalyser;
        _requestValidator = requestValidator;
        _getAllOptionsValidator = getAllOptionsValidator;
    }

    public async Task<SequenceAnalysis> CreateAsync(string uniprotId, CancellationToken token = default)
    {
        _requestValidator.ValidateAndThrow(uniprotId);

        var sequence =  await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient, token);
        var sequenceAnalysis = _sequenceAnalyser.Analyse(sequence);
        
        await _repository.CreateAsync(sequenceAnalysis, token);

        return sequenceAnalysis;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _repository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<SequenceAnalysis>> GetAllAsync(GetAllSequenceAnalysisOptions getAllOptions,
        CancellationToken token = default)
    {
        _getAllOptionsValidator.ValidateAndThrow(getAllOptions);
        
        return _repository.GetAllAsync(getAllOptions, token);
    }

    public Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _repository.GetByIdAsync(id, token);
    }

    public Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default)
    {
        return _repository.GetByUniprotIdAsync(uniprotId, token);
    }
}

using FluentValidation;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Services;
public class SequenceAnalysisService : ISequenceAnalysisService
{
    private readonly ISequenceAnalysisRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly IUniprotAPI _uniprotAPI;
    private readonly ISequenceAnalyser _sequenceAnalyser;
    private readonly IValidator<string> _requestValidator;

    public SequenceAnalysisService(ISequenceAnalysisRepository repository, 
        HttpClient httpClient, IUniprotAPI uniprotAPI, ISequenceAnalyser sequenceAnalyser, IValidator<string> requestValidator)
    {
        _repository = repository;
        _httpClient = httpClient;
        _uniprotAPI = uniprotAPI;
        _sequenceAnalyser = sequenceAnalyser;
        _requestValidator = requestValidator;
    }

    public async Task<SequenceAnalysis> CreateAsync(string uniprotId, CancellationToken token = default)
    {
        _requestValidator.ValidateAndThrow(uniprotId);
        
        //create and analyse sequence
        var sequence =  await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient);
        var sequenceAnalysis = _sequenceAnalyser.Analyse(sequence);
        
        await _repository.CreateAsync(sequenceAnalysis, token);

        return sequenceAnalysis;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _repository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<SequenceAnalysis>> GetAllAsync(CancellationToken token = default)
    {
        return _repository.GetAllAsync(token);
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

using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
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

    public SequenceAnalysisService(ISequenceAnalysisRepository repository, 
        HttpClient httpClient, IUniprotAPI uniprotAPI, ISequenceAnalyser sequenceAnalyser)
    {
        _repository = repository;
        _httpClient = httpClient;
        _uniprotAPI = uniprotAPI;
        _sequenceAnalyser = sequenceAnalyser;
    }

    public async Task<SequenceAnalysis> CreateAsync(string uniprotId)
    {
        //create and analyse sequence
        var sequence =  await _uniprotAPI.GetSequenceDetails(uniprotId, _httpClient);
        var sequenceAnalysis = _sequenceAnalyser.Analyse(sequence);
        
        await _repository.CreateAsync(sequenceAnalysis);

        return sequenceAnalysis;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _repository.DeleteByIdAsync(id);
    }

    public Task<IEnumerable<SequenceAnalysis>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<SequenceAnalysis?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId)
    {
        return _repository.GetByUniprotIdAsync(uniprotId);
    }
}

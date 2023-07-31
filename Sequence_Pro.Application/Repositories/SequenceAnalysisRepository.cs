using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Repositories;
public class SequenceAnalysisRepository : ISequenceAnalysisRepository
{
    private readonly List<SequenceAnalysis> _sequenceAnalyses = new();

    public Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis)
    {
        _sequenceAnalyses.Add(sequenceAnalysis);
        return Task.FromResult(true);
    }
    public Task<SequenceAnalysis?> GetByIdAsync(Guid id)
    {
        var sequenceAnalysis = _sequenceAnalyses.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(sequenceAnalysis);
    }

    public Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId)
    {
        var sequenceAnalysis = _sequenceAnalyses.FirstOrDefault(x => x.UniprotId == uniprotId);
        return Task.FromResult(sequenceAnalysis);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _sequenceAnalyses.RemoveAll(x => x.Id == id);
        var analysisRemoved = removedCount > 0;
        return Task.FromResult(analysisRemoved);
    }

    public Task<IEnumerable<SequenceAnalysis>> GetAllAsync()
    {
        return Task.FromResult(_sequenceAnalyses.AsEnumerable());
    }


}


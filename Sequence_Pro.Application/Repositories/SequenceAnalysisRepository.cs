using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Database.Mapping;
using Sequence_Pro.Application.Database.Models;
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
    private readonly SequenceProContext _dbContext;
    public SequenceAnalysisRepository(SequenceProContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis, CancellationToken token = default)
    {
        var sequenceAnalysisEntity = sequenceAnalysis.MapToEntity();
        await _dbContext.AddAsync(sequenceAnalysisEntity, token);
        var result = await _dbContext.SaveChangesAsync(token);

        return result > 0;
    }

    public async Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var sequenceAnalysisEntity = await _dbContext.SequenceAnalyses
            .Where(s => s.Id == id)
            .SingleOrDefaultAsync(token);

        if (sequenceAnalysisEntity == null)
        {
            return null;
        }
        return sequenceAnalysisEntity.MapToObject();
    }

    public async Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default)
    {
        var sequenceAnalysisEntity = await _dbContext.SequenceAnalyses
            .Where(s => s.UniprotId == uniprotId)
            .FirstOrDefaultAsync(token);

        if (sequenceAnalysisEntity == null)
        {
            return null;
        }
        return sequenceAnalysisEntity.MapToObject();
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SequenceAnalysis>> GetAllAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}

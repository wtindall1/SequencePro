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

    public async Task<IEnumerable<SequenceAnalysis>> GetAllAsync(CancellationToken token = default)
    {
        var sequenceAnalysisEntities = await _dbContext.SequenceAnalyses
            .ToListAsync(token);

        return sequenceAnalysisEntities.Select(x => x.MapToObject());
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        var entityToRemove = _dbContext.SequenceAnalyses
            .SingleOrDefault(x => x.Id == id);

        if (entityToRemove != null)
        {
            _dbContext.SequenceAnalyses.Remove(entityToRemove);
            await _dbContext.SaveChangesAsync(token);
            return true;
        }
        return false;  
    }
}

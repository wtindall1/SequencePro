using Microsoft.EntityFrameworkCore;
using SequencePro.Application.Database;
using SequencePro.Application.Database.Mapping;
using SequencePro.Application.Database.Models;
using SequencePro.Application.Interfaces;
using SequencePro.Application.Logging;
using SequencePro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Repositories;
public class SequenceAnalysisRepository : ISequenceAnalysisRepository
{
    private readonly SequenceProContext _dbContext;
    private readonly ILoggerAdapter _logger;
    public SequenceAnalysisRepository(SequenceProContext dbContext, ILoggerAdapter loggerAdapter)
    {
        _dbContext = dbContext;
        _logger = loggerAdapter;
    }

    public async Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis, CancellationToken token = default)
    {
        var sequenceAnalysisEntity = sequenceAnalysis.MapToEntity();
        
        try
        {
            await _dbContext.AddAsync(sequenceAnalysisEntity, token);
            var result = await _dbContext.SaveChangesAsync(token);
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving record to DB. Error: {1}", ex.Message);
            throw;
        }
    }

    public async Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        try
        {
            var sequenceAnalysisEntity = await _dbContext.SequenceAnalyses
                .Include(s => s.AminoAcidCompositions)
                .Where(s => s.Id == id)
                .SingleOrDefaultAsync(token);

            if (sequenceAnalysisEntity == null)
            {
                return null;
            }
            return sequenceAnalysisEntity.MapToObject();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving record from DB. Error: {1}", ex.Message);
            throw;
        }
    }

    public async Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default)
    {
        try
        {
            var sequenceAnalysisEntity = await _dbContext.SequenceAnalyses
            .Include(s => s.AminoAcidCompositions)
            .Where(s => s.UniprotId == uniprotId)
            .FirstOrDefaultAsync(token);

            if (sequenceAnalysisEntity == null)
            {
                return null;
            }
            return sequenceAnalysisEntity.MapToObject();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving record from DB. Error: {1}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<SequenceAnalysis>> GetAllAsync(GetAllSequenceAnalysisOptions getAllOptions,
        CancellationToken token = default)
    {
        try
        {
            IQueryable<SequenceAnalysisEntity> query = _dbContext.SequenceAnalyses
                .Include(s => s.AminoAcidCompositions);

            if (getAllOptions.UniprotId != null)
            {
                query = query.Where(x => x.UniprotId == getAllOptions.UniprotId);
            }

            if (getAllOptions.SortField != null)
            {
                query = getAllOptions.SortOrder == SortOrder.Descending ?
                    query.OrderByDescending(x => EF.Property<object>(x, getAllOptions.SortField)) :
                    query.OrderBy(x => EF.Property<object>(x, getAllOptions.SortField));
            }

            var sequenceAnalysisEntities = await query.ToListAsync(token);

            return sequenceAnalysisEntities.Select(x => x.MapToObject());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving records from DB. Error: {1}", ex.Message);
            throw;
        }        
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting record. Error: {1}", ex.Message);
            throw;
        }
    }
}

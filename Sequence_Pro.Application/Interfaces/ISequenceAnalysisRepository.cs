using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Interfaces;
public interface ISequenceAnalysisRepository
{
    Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis, CancellationToken token = default);

    Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default);

    Task<IEnumerable<SequenceAnalysis>> GetAllAsync(CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}

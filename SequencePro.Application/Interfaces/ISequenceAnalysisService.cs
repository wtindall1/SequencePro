using SequencePro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Interfaces;
public interface ISequenceAnalysisService
{
    Task<SequenceAnalysis> CreateAsync(string uniprotId, CancellationToken token = default);

    Task<SequenceAnalysis?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId, CancellationToken token = default);

    Task<IEnumerable<SequenceAnalysis>> GetAllAsync(GetAllSequenceAnalysisOptions getAllOptions,
        CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}

using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Interfaces;
public interface ISequenceAnalysisRepository
{
    Task<bool> CreateAsync(SequenceAnalysis sequenceAnalysis);

    Task<SequenceAnalysis?> GetByIdAsync(Guid id);

    Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId);

    Task<IEnumerable<SequenceAnalysis>> GetAllAsync();

    Task<bool> DeleteByIdAsync(Guid id);
}

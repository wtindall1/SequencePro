using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Interfaces;
public interface ISequenceAnalysisService
{
    Task<SequenceAnalysis> CreateAsync(string uniprotId);

    Task<SequenceAnalysis?> GetByIdAsync(Guid id);

    Task<SequenceAnalysis?> GetByUniprotIdAsync(string uniprotId);

    Task<IEnumerable<SequenceAnalysis>> GetAllAsync();

    Task<bool> DeleteByIdAsync(Guid id);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Contracts.Requests;
public class GetAllSequenceAnalysisRequest
{
    public string? UniprotId { get; init; }

    public string? SortBy { get; init; }
}

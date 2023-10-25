using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Contracts.Responses;
public class AllAnalysesResponse
{
    public required IEnumerable<SequenceAnalysisResponse> Items { get; init; } = Enumerable.Empty<SequenceAnalysisResponse>();
}

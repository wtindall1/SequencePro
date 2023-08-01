using Sequence_Pro.Application.Models;
using Sequence_Pro.Contracts.Responses;

namespace Sequence_Pro.API.Mapping;

public static class ContractMapping
{
    public static SequenceAnalysisResponse MapToResponse(this SequenceAnalysis sequenceAnalysis)
    {
        return new SequenceAnalysisResponse
        {
            Id = sequenceAnalysis.Id,
            UniprotId = sequenceAnalysis.UniprotId,
            ProteinSequence = sequenceAnalysis.ProteinSequence,
            SequenceLength = sequenceAnalysis.SequenceLength,
            MolecularWeight = sequenceAnalysis.MolecularWeight,
            AminoAcidComposition = sequenceAnalysis.AminoAcidComposition
        };
    }

    public static AllAnalysesResponse MapToResponse(this IEnumerable<SequenceAnalysis> allAnalyses)
    {
        return new AllAnalysesResponse
        {
            Items = allAnalyses.Select(MapToResponse)
        };

    }
}

using SequencePro.Application.Models;
using SequencePro.Contracts.Requests;
using SequencePro.Contracts.Responses;
using System.Net.NetworkInformation;

namespace SequencePro.API.Mapping;

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

    public static GetAllSequenceAnalysisOptions MapToOptions(this GetAllSequenceAnalysisRequest request)
    {
        return new GetAllSequenceAnalysisOptions
        {
            FilterByUniprotId = request.UniprotId,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('+') ? SortOrder.Ascending : SortOrder.Descending
        };
    }
}

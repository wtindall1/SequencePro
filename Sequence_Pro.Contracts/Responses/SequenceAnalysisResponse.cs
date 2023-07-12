namespace Sequence_Pro.Contracts.Responses;

public class SequenceAnalysisResponse
{
    public required string proteinSequence { get; set; }

    public required int sequenceLength { get; set; }

    public required double molecularWeight { get; set; }

    public required Dictionary<char, double> aminoAcidComposition { get; set; }
}
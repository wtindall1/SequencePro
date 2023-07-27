namespace Sequence_Pro.Contracts.Responses;

public class SequenceAnalysisResponse
{
    public required string UniprotId {  get; set; }
    
    public required string ProteinSequence { get; set; }

    public required int SequenceLength { get; set; }

    public required double MolecularWeight { get; set; }

    public required Dictionary<char, double> AminoAcidComposition { get; set; }
}
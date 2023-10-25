namespace SequencePro.Contracts.Responses;

public class SequenceAnalysisResponse
{
    public Guid Id { get; init; }
    
    public required string UniprotId {  get; init; }
    
    public required string ProteinSequence { get; init; }

    public required int SequenceLength { get; init; }

    public required double MolecularWeight { get; init; }

    public required Dictionary<char, double> AminoAcidComposition { get; init; }
}
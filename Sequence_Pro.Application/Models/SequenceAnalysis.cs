using System;

namespace Sequence_Pro.Application.Models;

public class SequenceAnalysis
{
    public required Guid Id { get; set; }

    public required string UniprotId { get; set; }

    public required string ProteinSequence { get; set; }

    public required int SequenceLength { get; set; }

    public required double MolecularWeight { get; set; }

    public required Dictionary<char, double> AminoAcidComposition { get; set; }
}
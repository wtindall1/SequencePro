using System;

namespace Sequence_Pro.Application.Models;

public class SequenceAnalysis
{
	public required string proteinSequence { get; set; }

    public required float hydrophobicity { get; set; }

    public required int sequenceLength { get; set; }

    public required float molecularWeight { get; set; }

    public required Dictionary<string, float> { get; set; }
}
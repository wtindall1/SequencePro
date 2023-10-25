using System;

namespace SequencePro.Application.Models;

public class Sequence
{

    public required string uniqueIdentifier { get; set; }

    public required string entryName { get; set; }

    public required string proteinName { get; set; }

    public required string organismName { get; set; }

    public required string aminoAcidSequence { get; set; }
}



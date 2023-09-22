using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Database.Models;

public class SequenceAnalysis
{
    public required Guid Id { get; set; }

    public required string UniprotId { get; set; }

    public required string ProteinSequence { get; set; }

    public required int SequenceLength { get; set; }

    public required double MolecularWeight { get; set; }

    public required Dictionary<char, double> AminoAcidComposition { get; set; }
}

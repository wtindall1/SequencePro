using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Database.Models;
public class AminoAcidComposition
{
    public required Guid Id { get; set; }
    public required Guid SequenceAnalysisId { get; set; }

    public required char AminoAcid { get; set; }

    public required double Proportion { get; set; }
}

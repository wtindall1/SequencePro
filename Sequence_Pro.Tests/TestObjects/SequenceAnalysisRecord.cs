using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.TestObjects;
public class SequenceAnalysisRecord
{
    public required Guid Id { get; init; }
    public required string UniprotId { get; init; }
    public required string ProteinSequence { get ; init; }
    public required int SequenceLength { get; init; }
    public required double MolecularWeight { get; init; }
    public required string AminoAcidComposition { get; init; }
}

using Sequence_Pro.Application.Database.Models;
using Sequence_Pro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.Database.Mapping;
public static class ModelMapping
{
    public static SequenceAnalysisEntity MapToEntity(this SequenceAnalysis sequenceAnalysis)
    {
        return new SequenceAnalysisEntity
        {
            Id = sequenceAnalysis.Id,
            UniprotId = sequenceAnalysis.UniprotId,
            MolecularWeight = sequenceAnalysis.MolecularWeight,
            AminoAcidComposition = sequenceAnalysis.AminoAcidComposition,
            SequenceLength = sequenceAnalysis.SequenceLength,
            ProteinSequence = sequenceAnalysis.ProteinSequence
        };
    }

    public static SequenceAnalysis MapToObject(this SequenceAnalysisEntity sequenceAnalysisEntity)
    {
        return new SequenceAnalysis
        {
            Id = sequenceAnalysisEntity.Id,
            UniprotId = sequenceAnalysisEntity.UniprotId,
            MolecularWeight = sequenceAnalysisEntity.MolecularWeight,
            AminoAcidComposition = sequenceAnalysisEntity.AminoAcidComposition,
            SequenceLength = sequenceAnalysisEntity.SequenceLength,
            ProteinSequence = sequenceAnalysisEntity.ProteinSequence
        };
    }
}

using SequencePro.Application.Database.Models;
using SequencePro.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Application.Database.Mapping;
public static class ModelMapping
{
    public static SequenceAnalysisEntity MapToEntity(this SequenceAnalysis sequenceAnalysis)
    {
        var aminoAcidCompositions = sequenceAnalysis.AminoAcidComposition.Keys
            .Select(x => new AminoAcidComposition
            {
                Id = Guid.NewGuid(),
                SequenceAnalysisId = sequenceAnalysis.Id,
                AminoAcid = x,
                Proportion = sequenceAnalysis.AminoAcidComposition[x]
            })
            .ToList();

        return new SequenceAnalysisEntity
        {
            Id = sequenceAnalysis.Id,
            UniprotId = sequenceAnalysis.UniprotId,
            MolecularWeight = sequenceAnalysis.MolecularWeight,
            SequenceLength = sequenceAnalysis.SequenceLength,
            ProteinSequence = sequenceAnalysis.ProteinSequence,
            AminoAcidCompositions = aminoAcidCompositions
        };
    }

    public static SequenceAnalysis MapToObject(this SequenceAnalysisEntity sequenceAnalysisEntity)
    {
        return new SequenceAnalysis
        {
            Id = sequenceAnalysisEntity.Id,
            UniprotId = sequenceAnalysisEntity.UniprotId,
            MolecularWeight = sequenceAnalysisEntity.MolecularWeight,
            SequenceLength = sequenceAnalysisEntity.SequenceLength,
            ProteinSequence = sequenceAnalysisEntity.ProteinSequence,
            
            AminoAcidComposition = sequenceAnalysisEntity.AminoAcidCompositions
            .ToDictionary(x => x.AminoAcid, x => x.Proportion)
        };
    }
}

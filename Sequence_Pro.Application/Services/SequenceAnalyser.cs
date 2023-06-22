using System;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;

namespace Sequence_Pro.Application.Services;

public class SequenceAnalyser : ISequenceAnalyser
{
    public SequenceAnalysis Analyse(Sequence sequence)
    {

        return new SequenceAnalysis
        {
            proteinSequence = sequence.aminoAcidSequence,
            //rest of analysis fields here
        };
    }

    private float CalculateHydrophobicity(Sequence sequence)
    {

    }

    private int CalculateSequenceLength(Sequence sequence)
    {

    }

    private float CalculateMolecularWeight(Sequence sequence) 
    {  
    }

    private float CalculateIsoelectricPoint(Sequence sequence)
    {
    }
}

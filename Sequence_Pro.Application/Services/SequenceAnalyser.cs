using System;
using System.Linq;
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
            sequenceLength = sequence.aminoAcidSequence.Length,
            aminoAcidComposition = CalculateAminoAcidComposition(sequence),
            molecularWeight = CalculateMolecularWeight(sequence)
        };
    }

    private double CalculateMolecularWeight(Sequence sequence) 
    {  
        //unit is Daltons
        var aminoAcidWeights = new Dictionary<char, double>
        {
            {'A', 89.09},
            {'R', 174.20},
            {'N', 132.12},
            {'D', 133.10},
            {'C', 121.15},
            {'E', 147.13},
            {'Q', 146.15},
            {'G', 75.07},
            {'H', 155.16},
            {'I', 131.17},
            {'L', 131.17},
            {'K', 146.19},
            {'M', 149.21},
            {'F', 165.19},
            {'P', 115.13},
            {'S', 105.09},
            {'T', 119.12},
            {'W', 204.23},
            {'Y', 181.19},
            {'V', 117.15}
        };

        double molecularWeight = 0;

        foreach (char aminoAcid in sequence.aminoAcidSequence)
        {
            try
            {
                molecularWeight += aminoAcidWeights[aminoAcid];
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Invalid Amino Acid in Sequence");
            }
        }

        return molecularWeight;
    }


    private Dictionary<char, double> CalculateAminoAcidComposition(Sequence sequence)
    {
        var aminoAcidComposition = new Dictionary<char, double>();

        char[] aminoAcids = { 'A', 'R', 'N', 'D', 'C', 'E', 'Q', 'G', 'H', 'I', 'L', 'K', 'M', 'F', 'P', 'S', 'T', 'W', 'Y', 'V' };

        foreach (char aminoAcid in aminoAcids)
        {
            var count = sequence.aminoAcidSequence.Where(x => x == aminoAcid).Count();
            aminoAcidComposition[aminoAcid] = (double)count / sequence.aminoAcidSequence.Length;
        }

        return aminoAcidComposition;


    }

}

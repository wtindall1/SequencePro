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
        //Daltons, average isotopic mass
        var aminoAcidWeights = new Dictionary<char, double>
        {
            {'A', 71.0788},
            {'R', 156.1875},
            {'N', 114.1038},
            {'D', 115.0886},
            {'C', 103.1388},
            {'E', 129.1155},
            {'Q', 128.1307},
            {'G', 57.0519},
            {'H', 137.1411},
            {'I', 113.1594},
            {'L', 113.1594},
            {'K', 128.1741},
            {'M', 131.1926},
            {'F', 147.1766},
            {'P', 97.1167},
            {'S', 87.0782},
            {'T', 101.1051},
            {'W', 186.2132},
            {'Y', 163.1760},
            {'V', 99.1326}
        };

        //starts with mass of a single water molecule (convention due to 1 water molecule released per peptide bond formed)
        double molecularWeight = 18.0153;

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

        return Math.Round(molecularWeight,2);
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

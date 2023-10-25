using System;
using Xunit;
using System.Runtime.CompilerServices;
using SequencePro.Application.Models;
using SequencePro.Application.Services;
using SequencePro.Unit.Tests.TestObjects;

namespace SequencePro.Unit.Tests.Unit;


public class Test_SequenceAnalyser
{
    private readonly Sequence _sequence;
    private readonly SequenceAnalyser _sequenceAnalyser;

    public Test_SequenceAnalyser()
    {
        //create sequence object to pass to Analyse()
        _sequence = new Sequence
        {
            uniqueIdentifier = "P12563",
            entryName = "HN_PI3HU",
            proteinName = "Hemagglutinin-neuraminidase",
            organismName = "Human parainfluenza 3 virus (strain Tex/9305/82)",
            aminoAcidSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS"
        };

        _sequenceAnalyser = new SequenceAnalyser();

    }

    [Fact]
    public void Test_SequenceAnalyser_Analyse_Molecular_Weight()
    {
        SequenceAnalysis analysis = _sequenceAnalyser.Analyse(_sequence);

        Assert.True(analysis.MolecularWeight == 64394.58, $"Molecular weight calculated was: {analysis.MolecularWeight}, vs expected: 64394.58");
    }

    [Fact]
    public void Test_SequenceAnalyser_Analyse_AminoAcid_Composition()
    {
        SequenceAnalysis analysis = _sequenceAnalyser.Analyse(_sequence);

        var expectedAminoAcidComposition = new Dictionary<char, double>
        {
            { 'A', 0.024 },
            { 'R', 0.042 },
            { 'N', 0.075 },
            { 'D', 0.056 },
            { 'C', 0.024 },
            { 'Q', 0.035 },
            { 'E', 0.035 },
            { 'G', 0.058 },
            { 'H', 0.026 },
            { 'I', 0.107 },
            { 'L', 0.087 },
            { 'K', 0.052 },
            { 'M', 0.019 },
            { 'F', 0.019 },
            { 'P', 0.051 },
            { 'S', 0.091 },
            { 'T', 0.082 },
            { 'W', 0.017 },
            { 'Y', 0.042 },
            { 'V', 0.056 },
        };

        Assert.True(DictionaryComparer.Equals(expectedAminoAcidComposition, analysis.AminoAcidComposition), "Calculated amino acid composition did not match expected.");
    }
}
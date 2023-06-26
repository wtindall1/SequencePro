using System;
using Xunit;
using System.Runtime.CompilerServices;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Services;


namespace Sequence_Pro.Tests;


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
    public async void Test_SequenceAnalyser_Analyse_Molecular_Weight()
    {
        SequenceAnalysis analysis = _sequenceAnalyser.Analyse(_sequence);

        Assert.True(analysis.molecularWeight == 64394.58, $"Molecular weight calculated was: {analysis.molecularWeight}, vs expected: 64394.58");
    }
}
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Text.Json;

namespace Sequence_Pro.Tests.TestObjects;
public static class SequenceAnalysisExample
{
    public static SequenceAnalysis Create()
    {
        return new SequenceAnalysis()
        {
            Id = Guid.NewGuid(),
            UniprotId = "P12563",
            ProteinSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS",
            SequenceLength = 572,
            MolecularWeight = 64394.58,
            AminoAcidComposition = new Dictionary<char, double>
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
                { 'V', 0.056 }
            }
        };
    }

    public static SequenceAnalysisRecord CreateSequenceAnalysisRecord()
    {
        return new SequenceAnalysisRecord
        {
            Id = Guid.NewGuid(),
            UniprotId = "P12563",
            ProteinSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS",
            SequenceLength = 572,
            MolecularWeight = 64394.58,
            AminoAcidComposition = JsonSerializer.Serialize(new Dictionary<char, double>
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
                { 'V', 0.056 }
            })
        };
    }
}

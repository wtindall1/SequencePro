using FluentValidation;
using Moq;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.Unit;
public class Test_SequenceAnalysisService
{
    private readonly SequenceAnalysisService _sut;
    private readonly Mock<ISequenceAnalysisRepository> _mockSequenceAnalysisRepository;
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<IUniprotAPI> _mockUniprotAPI;
    private readonly Mock<ISequenceAnalyser> _mockSequenceAnalyser;
    private readonly Mock<RequestValidator> _mockRequestValidator;

    public Test_SequenceAnalysisService()
    {
        _mockSequenceAnalysisRepository = new Mock<ISequenceAnalysisRepository>();
        _mockUniprotAPI = new Mock<IUniprotAPI>();
        _mockSequenceAnalyser = new Mock<ISequenceAnalyser>();
        _mockRequestValidator = new Mock<RequestValidator>();
        _mockHttpClient = new Mock<HttpClient>();

        _sut = new SequenceAnalysisService(
            _mockSequenceAnalysisRepository.Object,
            _mockHttpClient.Object,
            _mockUniprotAPI.Object,
            _mockSequenceAnalyser.Object,
            _mockRequestValidator.Object
            );
    }

    [Fact]
    public void Test_CreateAsync_Returns_SequenceAnalysis()
    {
        //arrange
        var sequence = new Sequence
        {
            uniqueIdentifier = "P12563",
            entryName = "HN_PI3HU",
            proteinName = "Hemagglutinin-neuraminidase",
            organismName = "Human parainfluenza 3 virus (strain Tex/9305/82)",
            aminoAcidSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS"
        };

        _mockUniprotAPI.Setup(x => x.GetSequenceDetails("P12563", _mockHttpClient.Object))
            .ReturnsAsync(sequence);
    }
}

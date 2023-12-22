using FluentValidation;
using Moq;
using SequencePro.Application.Interfaces;
using SequencePro.Application.Models;
using SequencePro.Application.Repositories;
using SequencePro.Application.Services;
using SequencePro.Application.Validators;
using SequencePro.Contracts.Responses;
using SequencePro.Unit.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Unit.Tests.ApplicationTests;
public class Test_SequenceAnalysisService
{
    private readonly SequenceAnalysisService _sut;
    private readonly Mock<ISequenceAnalysisRepository> _mockSequenceAnalysisRepository;
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly Mock<IUniprotAPI> _mockUniprotAPI;
    private readonly Mock<ISequenceAnalyser> _mockSequenceAnalyser;
    private readonly Mock<CreateSequenceAnalysisRequestValidator> _mockRequestValidator;

    private static string _uniprotId = "P12563";

    private static Sequence _sequence = new()
    {
        uniqueIdentifier = _uniprotId,
        entryName = "HN_PI3HU",
        proteinName = "Hemagglutinin-neuraminidase",
        organismName = "Human parainfluenza 3 virus (strain Tex/9305/82)",
        aminoAcidSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS"
    };

    private static SequenceAnalysis _sequenceAnalysis = SequenceAnalysisExample.Create();

    public Test_SequenceAnalysisService()
    {
        _mockSequenceAnalysisRepository = new Mock<ISequenceAnalysisRepository>();
        _mockUniprotAPI = new Mock<IUniprotAPI>();
        _mockSequenceAnalyser = new Mock<ISequenceAnalyser>();
        _mockRequestValidator = new Mock<CreateSequenceAnalysisRequestValidator>();
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
    public async Task Test_CreateAsync_Returns_SequenceAnalysis_Asynchronously()
    {
        //Arrange
        _mockUniprotAPI.Setup(x => x.GetSequenceDetails(_uniprotId, _mockHttpClient.Object, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sequence);

        _mockSequenceAnalyser.Setup(x => x.Analyse(_sequence))
            .Returns(_sequenceAnalysis);

        //Act
        var result = await _sut.CreateAsync(_uniprotId);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
    }

    [Fact]
    public async Task Test_GetByIdAsync_Returns_SequenceAnalysis_Asynchronously()
    {
        //Arrange
        var guid = Guid.NewGuid();
        _mockSequenceAnalysisRepository.Setup(x => x.GetByIdAsync(guid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sequenceAnalysis);

        //Act
        var result = await _sut.GetByIdAsync(guid);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
    }

    [Fact]
    public async Task Test_GetByUniprotIdAsync_Returns_SequenceAnalysis_Asynchronously()
    {
        //Arrange
        _mockSequenceAnalysisRepository.Setup(x => x.GetByUniprotIdAsync(_uniprotId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_sequenceAnalysis);

        //Act
        var result = await _sut.GetByUniprotIdAsync(_uniprotId);

        //Assert
        Assert.IsType<SequenceAnalysis>(result);
    }

    [Fact]
    public async Task Test_GetAllAsync_Returns_SequenceAnalysis_IEnumerable_Asynchronously()
    {
        //Arrange
        var analyses = new List<SequenceAnalysis>
        {
            _sequenceAnalysis,
            _sequenceAnalysis,
            _sequenceAnalysis,
        };

        _mockSequenceAnalysisRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(analyses);

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<SequenceAnalysis>>(result);
    }

    [Fact]
    public async Task Test_DeleteByIdAsync_Returns_Boolean_Asynchronously()
    {
        //Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        _mockSequenceAnalysisRepository.Setup(x => x.DeleteByIdAsync(guid1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockSequenceAnalysisRepository.Setup(x => x.DeleteByIdAsync(guid2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result1 = await _sut.DeleteByIdAsync(guid1);
        var result2 = await _sut.DeleteByIdAsync(guid2);

        //Assert
        Assert.True(result1);
        Assert.False(result2);
    }
}

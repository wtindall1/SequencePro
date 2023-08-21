using FluentValidation;
using Moq;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Validators;
using Sequence_Pro.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private static string _uniprotId = "P12563";

    private static Sequence _sequence = new()
    {
        uniqueIdentifier = _uniprotId,
        entryName = "HN_PI3HU",
        proteinName = "Hemagglutinin-neuraminidase",
        organismName = "Human parainfluenza 3 virus (strain Tex/9305/82)",
        aminoAcidSequence = "MEYWKHTNHRKDAGNELETSMATHGNKLTNKITYILWTIILVLLSIVLIIVLINSIKSEKAHESLLQDINNEFMEITEKIQMASDNTNDLIQSGVNTRLLTIQSHVQNYIPISLTQQMSDLRKFISEIIIRNDNQEVPPQRITHDVGIKPLNPDDFWRCTSGLPSLMKTPKIRLMPGPGLLTMPTTVDGCVRTPSLVINDLIYAYTSNLITRGCQDIGKSYQVLQIGIITVNSDLVPDLNPRISHTFNINDNRKSCSLALLNTDVYQLCSTPKVDERSDYASSGIEDIVLDIVNYDGSISTTRFKNNNISFDQPYAALYPSVGPGIYYKGKIIFLGYGGLEHPINENVICNTTGCPGKTQRDCNQASHSPWFSDRRMVNSIIVVDKGLNSIPKLKVWTISMRQNYWGSEGRLLLLGNKIYIYTRSTSWHSKLQLGIIDITDYSDIRIKWTWHNVLSRPGNNECPWGHSCPDGCITGVYTDAYPLNPTGSIVSSVILDSQKSRVNPVITYSTATERVNELAIRNKTLSAGYTTTSCITHYNKGYCFHIVEINHKSLDTFQPMLFKTEVPKSCS"
    };

    private static SequenceAnalysis _sequenceAnalysis = new()
    {
        Id = Guid.NewGuid(),
        UniprotId = _sequence.uniqueIdentifier,
        ProteinSequence = _sequence.aminoAcidSequence,
        SequenceLength = _sequence.aminoAcidSequence.Length,
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
    public async Task Test_CreateAsync_Returns_SequenceAnalysis_Asynchronously()
    {
        //Arrange
        _mockUniprotAPI.Setup(x => x.GetSequenceDetails(_uniprotId, _mockHttpClient.Object))
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
        _mockSequenceAnalysisRepository.Setup(x => x.GetByIdAsync(guid))
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
        _mockSequenceAnalysisRepository.Setup(x => x.GetByUniprotIdAsync(_uniprotId))
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

        _mockSequenceAnalysisRepository.Setup(x => x.GetAllAsync())
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
        _mockSequenceAnalysisRepository.Setup(x => x.DeleteByIdAsync(guid1))
            .ReturnsAsync(true);
        _mockSequenceAnalysisRepository.Setup(x => x.DeleteByIdAsync(guid2))
            .ReturnsAsync(false);

        //Act
        var result1 = await _sut.DeleteByIdAsync(guid1);
        var result2 = await _sut.DeleteByIdAsync(guid2);

        //Assert
        Assert.True(result1);
        Assert.False(result2);


    }
}

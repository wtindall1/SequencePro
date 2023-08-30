using Dapper;
using Moq;
using Moq.Dapper;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.Unit;
public class Test_SequenceAnalysisRepository
{
    private readonly SequenceAnalysisRepository _sut;
    private readonly Mock<IDbConnectionFactory> _mockConnectionFactory;
    private readonly Mock<IDbConnection> _mockConnection;
    private static SequenceAnalysis _sequenceAnalysis = SequenceAnalysisExample.Create();


    public Test_SequenceAnalysisRepository()
    {
        _mockConnection = new Mock<IDbConnection>();
        _mockConnection.Setup(x => x.BeginTransaction())
            .Returns(new Mock<IDbTransaction>().Object);
        _mockConnectionFactory = new Mock<IDbConnectionFactory>();
        _mockConnectionFactory.Setup(x => x.CreateConnectionAsync())
            .ReturnsAsync(_mockConnection.Object);

        _sut = new SequenceAnalysisRepository(_mockConnectionFactory.Object);
    }

    [Fact]
    public async Task Test_CreateAsync_Returns_True_When_Record_Created()
    {
        //Arrange
        _mockConnection.SetupDapperAsync(x => x.ExecuteAsync(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(1);

        //Act
        var result = await _sut.CreateAsync(_sequenceAnalysis);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Test_CreateAsync_Returns_False_When_No_Record_Created()
    {
        //Arrange
        _mockConnection.SetupDapperAsync(x => x.ExecuteAsync(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(0);

        //Act
        var result = await _sut.CreateAsync(_sequenceAnalysis);

        //Assert
        Assert.False(result);
    }
}

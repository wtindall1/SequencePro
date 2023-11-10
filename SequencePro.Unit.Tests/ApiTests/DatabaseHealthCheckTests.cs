using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SequencePro.Api.Health;
using SequencePro.Application.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Unit.Tests.ApiTests;

public class DatabaseHealthCheckTests
{
    private readonly Mock<DatabaseFacade> _mockDbFacade;
    private readonly Mock<SequenceProContext> _mockDbContext;
    private readonly DatabaseHealthCheck _sut;
    public DatabaseHealthCheckTests()
    {

        var dbContextOptions = new DbContextOptionsBuilder<SequenceProContext>();
        _mockDbContext = new Mock<SequenceProContext>(dbContextOptions.Options);

        _mockDbFacade = new Mock<DatabaseFacade>(_mockDbContext.Object);
        _mockDbContext.Setup(x => x.Database)
            .Returns(_mockDbFacade.Object);

        _sut = new DatabaseHealthCheck(_mockDbContext.Object);
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsHealthyStatus_WhenDbIsReachable()
    {
        //Arrange
        _mockDbFacade.Setup(x => x.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        //Act
        var result = await _sut.CheckHealthAsync(new HealthCheckContext());

        //Assert
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_ReturnsUnhealthyStatus_WhenDbIsUnreachable()
    {
        //Arrange
        _mockDbFacade.Setup(x => x.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result = await _sut.CheckHealthAsync(new HealthCheckContext());

        //Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}

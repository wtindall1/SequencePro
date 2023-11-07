using Autofac;
using Microsoft.EntityFrameworkCore;
using Moq;
using SequencePro.API.Controllers;
using SequencePro.Application.Database;
using SequencePro.Application.Interfaces;
using SequencePro.Application.IoC;
using SequencePro.Integration.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace SequencePro.Integration.Tests.Fixtures;
public class SequenceProApiTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
    new PostgreSqlBuilder()
    .WithDatabase("testdb")
    .WithUsername("user")
    .WithPassword("changeme")
    .Build();

    public SequenceProApiTestFixture()
    {
    }

    public IContainer? Container { get; set; }

    private IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule(new SequenceAnalysisModule(_dbContainer.GetConnectionString()));
        builder.Register(c => new HttpClient()).As<HttpClient>();
        builder.RegisterType<SequenceProController>().InstancePerLifetimeScope();

        var mockUniprotAPI = new Mock<IUniprotAPI>();
        mockUniprotAPI
            .Setup(x => x.GetSequenceDetails(
                It.IsAny<string>(),
                It.IsAny<HttpClient>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(SequenceExample.P12345());
        builder.RegisterInstance(mockUniprotAPI.Object).As<IUniprotAPI>();

        return builder.Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Container = BuildContainer();
    }
    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

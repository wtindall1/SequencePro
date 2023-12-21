using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using SequencePro.Application.Database;
using SequencePro.Application.Interfaces;
using SequencePro.Application.IoC;
using SequencePro.Integration.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace SequencePro.Integration.Tests.Common;
public abstract class BaseSequenceProApiServerFixture : IAsyncLifetime
{
    private readonly AutofacWebApiServerFactory<Program> _factory = new();
    private readonly PostgreSqlContainer _dbContainer;

    public BaseSequenceProApiServerFixture()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithDatabase(this.GetType().Name + "-db")
            .WithUsername("user")
            .WithPassword("changeme")
            .Build();
    }

    public SequenceProContext CreateTestDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SequenceProContext>();
        optionsBuilder.UseNpgsql(_dbContainer.GetConnectionString());
        return new SequenceProContext(optionsBuilder.Options);
    }

    public HttpClient CreateClient()
    {
        return _factory
            .ConfigureTestContainer(ConfigureServices)
            .CreateClient();
    }

    public void ConfigureServices(ContainerBuilder builder)
    {
        builder.RegisterModule(new SequenceAnalysisModule(
            _dbContainer.GetConnectionString()));

        var mockUniprotAPI = new Mock<IUniprotAPI>();
        mockUniprotAPI
            .Setup(x => x.GetSequenceDetails(
                It.IsAny<string>(),
                It.IsAny<HttpClient>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(SequenceExample.P12345());
        builder.RegisterInstance(mockUniprotAPI.Object).As<IUniprotAPI>();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }
    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

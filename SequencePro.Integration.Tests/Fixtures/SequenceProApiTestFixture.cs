using Autofac;
using Microsoft.EntityFrameworkCore;
using SequencePro.API.Controllers;
using SequencePro.Application.Database;
using SequencePro.Application.IoC;
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

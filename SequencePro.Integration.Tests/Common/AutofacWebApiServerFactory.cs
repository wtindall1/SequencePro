using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePro.Integration.Tests.Common;

/// <summary>
/// Based upon https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public class AutofacWebApiServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseServiceProviderFactory<ContainerBuilder>(new CustomServiceProviderFactory());
        return base.CreateHost(builder);
    }
}

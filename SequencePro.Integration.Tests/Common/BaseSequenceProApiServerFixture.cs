using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace SequencePro.Integration.Tests.Common;
public abstract class BaseSequenceProApiServerFixture
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

    public HttpClient CreateClient()
    {
        return _factory.CreateClient();
    }
}

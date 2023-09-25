using Dapper;
using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Database.Mapping;
using Sequence_Pro.Application.Database.Models;
using Sequence_Pro.Application.Models;
using Sequence_Pro.Tests.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Tests.TestDatabase;
public class TestDbManager
{
    private readonly SequenceProContext _dbContext;

    public TestDbManager(string connectionString)
    {
        _dbContext = new SequenceProContext(new DbContextOptionsBuilder<SequenceProContext>()
            .UseNpgsql(connectionString)
            .Options);
    }

    public async Task InitialiseDatabaseWithFiveRecordsAsync()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        //create 5 entities
        var entities = new List<SequenceAnalysisEntity>();
        for (int i = 0; i < 5; i++)
        {
            entities.Add(SequenceAnalysisExample.Create().MapToEntity());
        }

        _dbContext.AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }
}

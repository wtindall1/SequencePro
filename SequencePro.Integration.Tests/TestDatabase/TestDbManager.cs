using Microsoft.EntityFrameworkCore;
using SequencePro.Application.Database;
using SequencePro.Application.Database.Mapping;
using SequencePro.Application.Database.Models;
using SequencePro.Integration.Tests.TestObjects;

namespace SequencePro.Integration.Tests.TestDatabase;
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

using Microsoft.Extensions.Diagnostics.HealthChecks;
using SequencePro.Application.Database;

namespace SequencePro.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    public const string Name = "Database";
    
    private readonly SequenceProContext _dbContext;
    public DatabaseHealthCheck(SequenceProContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = await _dbContext.Database.CanConnectAsync(cancellationToken);

        if(isHealthy)
        {
            return HealthCheckResult.Healthy();
        }
        return HealthCheckResult.Unhealthy("Database connection failed.");
    }
}

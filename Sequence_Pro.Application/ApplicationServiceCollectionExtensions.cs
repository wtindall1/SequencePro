using Microsoft.Extensions.DependencyInjection;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Database;

namespace Sequence_Pro.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IUniprotAPI, UniprotAPI>();
        services.AddSingleton<ISequenceAnalyser, SequenceAnalyser>();
        services.AddSingleton<ISequenceAnalysisRepository, SequenceAnalysisRepository>();
        services.AddSingleton<ISequenceAnalysisService, SequenceAnalysisService>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));

        services.AddSingleton<DBInitialiser>();
        return services;
    }
}

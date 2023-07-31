using Microsoft.Extensions.DependencyInjection;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Repositories;

namespace Sequence_Pro.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IUniprotAPI, UniprotAPI>();
        services.AddSingleton<ISequenceAnalyser, SequenceAnalyser>();
        services.AddSingleton<ISequenceAnalysisRepository, SequenceAnalysisRepository>();
        return services;
    }
}

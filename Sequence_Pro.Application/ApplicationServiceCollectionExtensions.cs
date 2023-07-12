using Microsoft.Extensions.DependencyInjection;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Interfaces;

namespace Sequence_Pro.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IUniprotAPI, UniprotAPI>();
        services.AddSingleton<ISequenceAnalyser, SequenceAnalyser>();
        return services;
    }
}

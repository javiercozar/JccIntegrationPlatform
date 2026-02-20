using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Infraestructure.Configuration;

/// <summary>
/// Dependency injection configuration for the Commons platform.
/// Note: This file will be extended with auto-generated methods in DependencyInjectionConfig.g.cs
/// </summary>
public static partial class DependencyInjectionConfig
{
    /// <summary>
    /// Registers the Commons services in the DI container.
    /// Automatically includes all QueryHandlers, CommandHandlers, and gRPC services.
    /// </summary>
    public static IServiceCollection AddCommonsServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Register QueryHandlers automatically detected by the source generator
        services.AddGeneratedQueryHandlers();

        // Register CommandHandlers automatically detected by the source generator
        services.AddGeneratedCommandHandlers();

        // Register gRPC services
        services.AddGrpcServices();

        return services;
    }
}
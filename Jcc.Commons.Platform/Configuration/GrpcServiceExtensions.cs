using Grpc.AspNetCore.Server;
using Jcc.Commons.Platform.Endpoints.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Configuration;

/// <summary>
/// Extensions for configuring gRPC services in the application.
/// </summary>
public static class GrpcServiceExtensions
{
    /// <summary>
    /// Registers the necessary gRPC services in the dependency injection container.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Register gRPC services
        services.AddGrpc(options =>
        {
            // Configuration of default timeouts
            options.MaxSendMessageSize = 100 * 1024 * 1024; // 100MB
            options.MaxReceiveMessageSize = 100 * 1024 * 1024; // 100MB
        });

        // Register service implementations
        services.AddScoped<SearchAvailabilityService>();

        return services;
    }

    /// <summary>
    /// Configures the gRPC endpoints for the application.
    /// Should be called in the middleware pipeline configuration.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static void MapGrpcServices(this WebApplication app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        // Map gRPC services
        app.MapGrpcService<SearchAvailabilityService>();

        // gRPC test endpoint
        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync(
                "gRPC server is active. Use a gRPC client to communicate with the available services.");
        });
    }
}

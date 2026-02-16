using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Configuration;

/// <summary>
/// Configuración de inyección de dependencias para la plataforma de Commons.
/// Nota: Este archivo será extendido con métodos auto-generados en DependencyInjectionConfig.g.cs
/// </summary>
public static partial class DependencyInjectionConfig
{
    /// <summary>
    /// Registra los servicios de Commons en el contenedor DI.
    /// Incluye automáticamente todos los QueryHandlers detectados por el Source Generator.
    /// </summary>
    public static IServiceCollection AddPCommonsServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Registra los QueryHandlers detectados automáticamente por el source generator
        services.AddGeneratedQueryHandlers();

        // Registra los CommandHandlers detectados automáticamente por el source generator
        services.AddGeneratedCommandHandlers();
        return services;
    }
}
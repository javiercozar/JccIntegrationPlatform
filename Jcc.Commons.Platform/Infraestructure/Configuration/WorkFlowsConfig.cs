using Jcc.Commons.Platform.Application.Search.Steps;
using Jcc.Commons.Platform.Domain.Search;
using Jcc.Commons.Platform.Domain.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Infraestructure.Configuration;

public static class WorkFlowsConfig
{
    /// <summary>
    /// Registers the workflows in the DI container.
    /// </summary>
    public static IServiceCollection AddWorkFlows(this IServiceCollection services)
    {
        services.AddScoped<ValidateSupplierSearchRequestStep>();
        services.AddScoped<ValidateSupplierSearchResponseStep>();
        
        services.AddSingleton(typeof(IWorkflowBuilder<>), typeof(WorkFlowBuilder<>));
        services.AddSingleton<IWorkFlowExecutor<SearchContext>>(sp => {
            var builder = sp.GetRequiredService<IWorkflowBuilder<SearchContext>>();

            var workFlow = builder
                .AddStep<ValidateSupplierSearchRequestStep>()
                .AddStep<ValidateSupplierSearchResponseStep>()
                .Build();
            
            return workFlow;
        });

        return services;
    }
}
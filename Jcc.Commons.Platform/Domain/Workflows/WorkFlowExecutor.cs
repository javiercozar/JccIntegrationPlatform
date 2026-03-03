using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Domain.Workflows;

public sealed class WorkFlowExecutor<TContext> : IWorkFlowExecutor<TContext>
{
    private readonly IServiceScopeFactory scopeFactory;
    private const string GetRequiredServiceMethodName = nameof(ServiceProviderServiceExtensions.GetRequiredService);
    private readonly Type workflowStepType = typeof(IWorkflowStep<>);

    private readonly MethodInfo getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions)
        .GetMethod(name: GetRequiredServiceMethodName, types: [typeof(IServiceProvider), typeof(Type)])!;

    private readonly string WorkFlowStepExecuteMethodName = nameof(IWorkflowStep<>.Execute);
    private readonly ParameterExpression contextParameter = Expression.Parameter(typeof(TContext), "context");

    private readonly ParameterExpression serviceProviderParameter =
        Expression.Parameter(typeof(IServiceProvider), "serviceProvider");

    private readonly List<Func<TContext, IServiceProvider, ValueTask<bool>>> compiledSteps = new();

    public WorkFlowExecutor(IReadOnlyCollection<Type> stepsTypes, IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
        foreach (var stepType in stepsTypes)
        {
            compiledSteps.Add(compiledStep(stepType));
        }
    }

    public async ValueTask<bool> ExecuteAsync(TContext context)
    {
        using var scope = scopeFactory.CreateScope();

        foreach (var compiledStep in compiledSteps)
        {
            if (!await compiledStep(context, scope.ServiceProvider))
            {
                return false;
            }
        }

        return true;
    }

    private Func<TContext, IServiceProvider, ValueTask<bool>> compiledStep(Type stepType)
    {
        var stepInterface = stepType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == workflowStepType);
        var contextType = stepInterface.GetGenericArguments()[0];
        var getServiceCall = Expression.Call(
            null,
            getRequiredServiceMethod,
            serviceProviderParameter,
            Expression.Constant(stepType));

        var stepInstance = Expression.Convert(getServiceCall, stepType);
        var castContext = Expression.Convert(contextParameter, contextType);
        var executeCall = Expression.Call(
            stepInstance,
            stepInterface.GetMethod(WorkFlowStepExecuteMethodName)!,
            castContext);

        return Expression.Lambda<Func<TContext, IServiceProvider, ValueTask<bool>>>(
            executeCall,
            contextParameter,
            serviceProviderParameter).Compile();
    }
}
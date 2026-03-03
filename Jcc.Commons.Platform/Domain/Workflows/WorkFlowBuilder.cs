using Microsoft.Extensions.DependencyInjection;

namespace Jcc.Commons.Platform.Domain.Workflows;

public sealed class WorkFlowBuilder<TContext>(IServiceScopeFactory scopeFactory) : IWorkflowBuilder<TContext>
{
    private readonly List<Type> _stepTypes = new();

    public IWorkflowBuilder<TContext> AddStep<TStep>() where TStep : class
    {
        _stepTypes.Add(typeof(TStep));
        return this;
    }

    public WorkFlowExecutor<TContext> Build() => new(_stepTypes, scopeFactory);
}
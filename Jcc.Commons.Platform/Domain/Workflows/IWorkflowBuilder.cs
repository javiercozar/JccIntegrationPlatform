namespace Jcc.Commons.Platform.Domain.Workflows;

public interface IWorkflowBuilder<TContext>
{
    public IWorkflowBuilder<TContext> AddStep<TStep>() where TStep : class;
    
    /*
    public IWorkflowBuilder<TContext> If<TStep>(
        WorkflowCondition<TContext> condition, 
        Action<IWorkflowBuilder<TContext>> then) where TStep : class;
        */
    
    public WorkFlowExecutor<TContext> Build();
}
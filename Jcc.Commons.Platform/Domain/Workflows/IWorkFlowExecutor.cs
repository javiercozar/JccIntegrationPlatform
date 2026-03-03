namespace Jcc.Commons.Platform.Domain.Workflows;

public interface IWorkFlowExecutor<TContext>
{
    public ValueTask<bool> ExecuteAsync(TContext context);
}
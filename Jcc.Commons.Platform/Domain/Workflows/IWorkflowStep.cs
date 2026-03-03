namespace Jcc.Commons.Platform.Domain.Workflows;

public interface IWorkflowStep<in TStepContext>
{
    public ValueTask<bool> Execute(TStepContext context);
}
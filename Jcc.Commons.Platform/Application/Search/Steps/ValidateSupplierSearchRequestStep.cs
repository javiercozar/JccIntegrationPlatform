using Jcc.Commons.Platform.Domain.Search;
using Jcc.Commons.Platform.Domain.Workflows;

namespace Jcc.Commons.Platform.Application.Search.Steps;

public sealed class ValidateSupplierSearchRequestStep : IWorkflowStep<IValidateSupplierSearchRequest>
{
    public ValueTask<bool> Execute(IValidateSupplierSearchRequest context)
    {
        context.ValidRequest = true;
        return new ValueTask<bool>(true);
    }
}
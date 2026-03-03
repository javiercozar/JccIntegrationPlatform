using Jcc.Commons.Platform.Domain.Search;
using Jcc.Commons.Platform.Domain.Workflows;

namespace Jcc.Commons.Platform.Application.Search.Steps;

public sealed class ValidateSupplierSearchResponseStep : IWorkflowStep<IValidateSupplierSearchResponse>
{
    public ValueTask<bool> Execute(IValidateSupplierSearchResponse context)
    {
        context.ValidResponse = true;
        return new ValueTask<bool>(true);
    }
}
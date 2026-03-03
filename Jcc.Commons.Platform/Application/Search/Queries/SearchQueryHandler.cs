using Jcc.Commons.Platform.Domain;
using Jcc.Commons.Platform.Domain.Search;
using Jcc.Commons.Platform.Domain.Workflows;

namespace Jcc.Commons.Platform.Application.Search.Queries;

public sealed class SearchQueryHandler(IWorkFlowExecutor<SearchContext> workFlowExecutor) 
    : IQueryHandler<SearchQueryRequest, SearchQueryResponse>
{
    public async Task<SearchQueryResponse> HandleAsync(SearchQueryRequest request)
    {
        var searchContext = new SearchContext();
        var success = await workFlowExecutor.ExecuteAsync(searchContext);
        return new SearchQueryResponse(success);
    }
}
using Jcc.Commons.Platform.Domain;

namespace Jcc.Commons.Platform.Application.Search.Queries;

public sealed class SearchQueryHandler : IQueryHandler<SearchQueryRequest, SearchQueryResponse> 
{
    public Task<SearchQueryResponse> HandleAsync(SearchQueryRequest request) 
    {      
        return Task.FromResult(new SearchQueryResponse());
    }
}
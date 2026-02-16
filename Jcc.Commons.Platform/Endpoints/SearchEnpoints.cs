using Jcc.Commons.Platform.Application.Search;
using Jcc.Commons.Platform.Contracts.Search;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Jcc.Commons.Platform.Endpoints;

public static class SearchEnpoints {
    extension(IEndpointRouteBuilder enpoint) {
        public IEndpointRouteBuilder MapSearchEndpoints() {
            enpoint
                .MapPost("/api/search", Handler)
                .WithName("Search availability endpoint");
            
            return enpoint;
        }

        private async Task<IResult> Handler(SearchQueryHandler searchQueryHandler, SearchRequest request) {
            var queryRequest = new SearchQueryRequest(
                request.HotelCodes,
                request.CheckIn,
                request.CheckOut,
                request.Occupancy,
                request.Nationality,
                request.Currency,
                request.Language
            );
            
            var response = await searchQueryHandler.HandleAsync(queryRequest);

            return Results.Ok(response);
        }
    }
}
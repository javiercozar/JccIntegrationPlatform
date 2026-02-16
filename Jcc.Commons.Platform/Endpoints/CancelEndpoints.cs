using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Jcc.Commons.Platform.Endpoints;

public static class CancelEndpoints {
    extension(IEndpointRouteBuilder endpoint) {
        public IEndpointRouteBuilder MapCancelEndpoints() {
            endpoint
                .MapPost("/api/cancel", () => "Cancel Endpoint")
                .WithName("Cancel booking endpoint");
            
            return endpoint;
        }
    }
}
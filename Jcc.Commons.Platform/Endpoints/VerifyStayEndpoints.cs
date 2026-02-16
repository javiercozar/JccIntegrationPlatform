using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Jcc.Commons.Platform.Endpoints;

public static class VerifyStayEndpoints {
    extension(IEndpointRouteBuilder endpoint) {
        public IEndpointRouteBuilder MapVerifyStayEndpoints() {
            endpoint
                .MapPost("/api/verifystay", () => "Verify Stay Endpoint")
                .WithName("verifystay endpoint");
            
            return endpoint;
        }
    }
}
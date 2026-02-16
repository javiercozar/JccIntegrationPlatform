using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Jcc.Commons.Platform.Endpoints;

public static class BookEndpoints {
    extension(IEndpointRouteBuilder endpoint) {
        public IEndpointRouteBuilder MapBookEndpoints() {
            endpoint
                .MapPost("/api/book", () => "Book Endpoint")
                .WithName("Create Booking");
            
            return endpoint;
        }
        
        public IEndpointRouteBuilder MapGetBookingDetailsEndpoint() {
            endpoint
                .MapGet("/api/book", () => $"Booking Details for").
                WithName("Get Booking details");
            
            return endpoint;
        }
    }
}
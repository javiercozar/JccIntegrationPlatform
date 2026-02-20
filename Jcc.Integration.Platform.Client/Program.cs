using Grpc.Core;
using Grpc.Net.Client;
using Jcc.Commons.Platform.Grpc.Availability;

var options = new ClientOptions(new Uri("https://localhost:5000"), true);

if (options.Address.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
{
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
}

using var handler = new HttpClientHandler();
if (options.AllowUntrusted)
{
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
}

using var channel = GrpcChannel.ForAddress(options.Address, new GrpcChannelOptions {
    HttpClient = new HttpClient(handler)
});

var client = new Availability.AvailabilityClient(channel);
var request = options.BuildSampleRequest();

try
{
    using var call = client.Search(request);
    await foreach (var response in call.ResponseStream.ReadAllAsync())
    {
        options.PrintResponse(response);
    }

    return 0;
}
catch (RpcException ex)
{
    Console.Error.WriteLine($"gRPC error: {ex.Status.StatusCode} - {ex.Status.Detail}");
    return 2;
}

record ClientOptions(Uri Address, bool AllowUntrusted)
{
    public SearchRequest BuildSampleRequest()
    {
        var request = new SearchRequest {
            CredentialId = "demo-credential",
            TimeoutMilliseconds = 30_000,
            Stay = new StayPeriod {
                CheckIn = "2026-03-10",
                CheckOut = "2026-03-12"
            }
        };

        request.HotelIds.AddRange(new[] {
            "HTL-1001",
            "HTL-2002"
        });

        request.Occupancies.Add(new Occupancy { Ages = { 30, 28 } });
        request.Occupancies.Add(new Occupancy { Ages = { 6 } });

        return request;
    }

    public void PrintResponse(SearchResponse response)
    {
        var metadata = response.Metadata;
        Console.WriteLine(
            $"Batch: {metadata?.ResultsInThisBatch ?? 0}, Last: {metadata?.IsLastBatch ?? false}, Provider: {metadata?.ProviderName ?? "n/a"}");

        foreach (var hotel in response.HotelResult)
        {
            Console.WriteLine($"Hotel {hotel.HotelId}: {hotel.HotelName} ({hotel.Options.Count} options)");

            foreach (var option in hotel.Options)
            {
                var price = option.TotalPrice is null
                    ? "n/a"
                    : $"{option.TotalPrice.Amount:0.00} {option.TotalPrice.Currency}";

                Console.WriteLine(
                    $"  Option {option.OptionToken} | Refundable: {option.IsRefundable} | Total: {price}");
            }
        }
    }
};
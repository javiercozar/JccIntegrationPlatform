using Grpc.Core;
using HotelBooking.Grpc.SearchAvailability;
using Microsoft.Extensions.Logging;

namespace Jcc.Commons.Platform.Endpoints.Grpc;

/// <summary>
/// Implementation of the SearchAvailability gRPC service.
/// Provides hotel availability search based on specified criteria.
/// </summary>
public class SearchAvailabilityService : SearchAvailability.SearchAvailabilityBase
{
    private readonly Microsoft.Extensions.Logging.ILogger<SearchAvailabilityService> _logger;

    public SearchAvailabilityService(ILogger<SearchAvailabilityService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Searches for available hotels based on the provided criteria.
    /// Returns a stream of search results.
    /// </summary>
    /// <param name="request">Search criteria including stay period, occupancy, etc.</param>
    /// <param name="responseStream">Stream to send search results to the client.</param>
    /// <param name="context">Context of the gRPC call.</param>
    public override async Task Search(
        Contracts.Search.SearchRequest request,
        IAsyncStreamWriter<SearchResponse> responseStream,
        ServerCallContext context)
    {
        try
        {
            _logger.LogInformation(
                "Starting availability search for {HotelCount} hotels with credential {CredentialId}",
                request.HotelIds.Count,
                request.CredentialId);

            // Validate input
            if (request == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "SearchRequest cannot be null"));
            }

            if (string.IsNullOrWhiteSpace(request.CredentialId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "credential_id is required"));
            }

            if (request.HotelIds.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "At least one hotel_id must be provided"));
            }

            if (request.Stay == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "stay period is required"));
            }

            if (request.Occupancies.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "At least one occupancy must be provided"));
            }

            // TODO: Implement the actual search logic
            // For now, returns a sample result
            var response = new SearchResponse
            {
                Metadata = new HotelBooking.Common.Metadata
                {
                    IsLastBatch = true,
                    ProviderName = "Demo Provider",
                    ResultsInThisBatch = 1
                }
            };

            response.HotelResult.Add(new HotelResult
            {
                HotelId = "HOTEL_001",
                HotelName = "Demo Hotel",
                Options =
                {
                    new BookingOption
                    {
                        OptionToken = "TOKEN_001",
                        IsRefundable = true,
                        Remarks = "Available for booking",
                        TotalPrice = new HotelBooking.Common.Money
                        {
                            Currency = "EUR",
                            Amount = 150.00
                        }
                    }
                }
            });

            await responseStream.WriteAsync(response);

            _logger.LogInformation("Search completed successfully");
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during availability search");
            throw new RpcException(new Status(StatusCode.Internal, "Internal error during search"));
        }
    }
}

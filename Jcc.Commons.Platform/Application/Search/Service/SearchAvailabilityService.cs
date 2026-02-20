using Grpc.Core;
using Jcc.Commons.Platform.Application.Search.Queries;
using Jcc.Commons.Platform.Grpc.Availability;
using Metadata = Jcc.Commons.Platform.Grpc.Availability.Metadata;

namespace Jcc.Commons.Platform.Application.Search.Service;

public class SearchAvailabilityService(SearchQueryHandler searchQueryHandler) : Availability.AvailabilityBase
{
    // Implement gRPC service methods here
    public override async Task Search(SearchRequest request, IServerStreamWriter<SearchResponse> responseStream, ServerCallContext context)
    {
        //searchQueryHandler.HandleAsync(request, responseStream, context);
        
        // Generate some dummy data for testing
        var hotelResults = new List<HotelResult>
        {
            new HotelResult
            {
                HotelId = "HOTEL001",
                HotelName = "Grand Plaza Hotel",
                Options =
                {
                    new BookingOption
                    {
                        OptionToken = "OPT-001-A",
                        TotalPrice = new Money { Currency = "EUR", Amount = 250.00 },
                        IsRefundable = true,
                        Remarks = "Breakfast included",
                        CancellationPolicies =
                        {
                            new CancellationPolicy
                            {
                                FromDate = "2026-02-10",
                                Penalty = new Money { Currency = "EUR", Amount = 50.00 }
                            }
                        },
                        Taxes =
                        {
                            new Tax
                            {
                                Code = "VAT",
                                Amount = new Money { Currency = "EUR", Amount = 25.00 },
                                IncludedInPrice = true
                            }
                        },
                        Supplements =
                        {
                            new Supplement
                            {
                                Code = "WIFI",
                                Description = "Free WiFi",
                                Amount = new Money { Currency = "EUR", Amount = 0.00 }
                            }
                        },
                        Rooms =
                        {
                            new RoomDetail
                            {
                                RoomCode = "DBL",
                                RoomName = "Double Room",
                                BoardCode = "BB",
                                BoardName = "Bed & Breakfast",
                                Occupancy = new Occupancy { Ages = { 30, 28 } }
                            }
                        }
                    },
                    new BookingOption
                    {
                        OptionToken = "OPT-001-B",
                        TotalPrice = new Money { Currency = "EUR", Amount = 180.00 },
                        IsRefundable = false,
                        Remarks = "Non-refundable rate",
                        CancellationPolicies =
                        {
                            new CancellationPolicy
                            {
                                FromDate = "2026-02-01",
                                Penalty = new Money { Currency = "EUR", Amount = 180.00 }
                            }
                        },
                        Taxes =
                        {
                            new Tax
                            {
                                Code = "VAT",
                                Amount = new Money { Currency = "EUR", Amount = 18.00 },
                                IncludedInPrice = true
                            }
                        },
                        Rooms =
                        {
                            new RoomDetail
                            {
                                RoomCode = "SGL",
                                RoomName = "Single Room",
                                BoardCode = "RO",
                                BoardName = "Room Only",
                                Occupancy = new Occupancy { Ages = { 30 } }
                            }
                        }
                    }
                }
            },
            new HotelResult
            {
                HotelId = "HOTEL002",
                HotelName = "Sunset Beach Resort",
                Options =
                {
                    new BookingOption
                    {
                        OptionToken = "OPT-002-A",
                        TotalPrice = new Money { Currency = "EUR", Amount = 350.00 },
                        IsRefundable = true,
                        Remarks = "All inclusive package",
                        CancellationPolicies =
                        {
                            new CancellationPolicy
                            {
                                FromDate = "2026-02-15",
                                Penalty = new Money { Currency = "EUR", Amount = 0.00 }
                            },
                            new CancellationPolicy
                            {
                                FromDate = "2026-02-20",
                                Penalty = new Money { Currency = "EUR", Amount = 100.00 }
                            }
                        },
                        Taxes =
                        {
                            new Tax
                            {
                                Code = "VAT",
                                Amount = new Money { Currency = "EUR", Amount = 35.00 },
                                IncludedInPrice = true
                            },
                            new Tax
                            {
                                Code = "CITY_TAX",
                                Amount = new Money { Currency = "EUR", Amount = 15.00 },
                                IncludedInPrice = false
                            }
                        },
                        Supplements =
                        {
                            new Supplement
                            {
                                Code = "SPA",
                                Description = "Spa Access",
                                Amount = new Money { Currency = "EUR", Amount = 30.00 }
                            },
                            new Supplement
                            {
                                Code = "PARKING",
                                Description = "Private Parking",
                                Amount = new Money { Currency = "EUR", Amount = 20.00 }
                            }
                        },
                        Rooms =
                        {
                            new RoomDetail
                            {
                                RoomCode = "SUI",
                                RoomName = "Ocean View Suite",
                                BoardCode = "AI",
                                BoardName = "All Inclusive",
                                Occupancy = new Occupancy { Ages = { 35, 32, 8, 5 } }
                            }
                        }
                    }
                }
            }
        };
        
        // Send first batch
        await responseStream.WriteAsync(new SearchResponse
        {
            Metadata = new Metadata
            {
                IsLastBatch = false,
                ProviderName = "TestProvider1",
                ResultsInThisBatch = 1
            },
            HotelResult = { hotelResults[0] }
        });
        
        // Simulate some delay
        await Task.Delay(100);
        
        // Send second batch
        await responseStream.WriteAsync(new SearchResponse
        {
            Metadata = new Metadata
            {
                IsLastBatch = true,
                ProviderName = "TestProvider2",
                ResultsInThisBatch = 1
            },
            HotelResult = { hotelResults[1] }
        });
    }
}
using Jcc.Commons.Platform.Grpc.Availability;

namespace Jcc.Commons.Platform.Application.Search.Queries;

public sealed record SearchQueryRequest(
    string[] HotelCodes,
    DateOnly CheckIn,
    DateOnly CheckOut,
    Occupancy Occupancy,
    string? Nationality = null,
    string? Currency = null,
    string? Language = null);
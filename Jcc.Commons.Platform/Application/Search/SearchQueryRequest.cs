using Jcc.Commons.Platform.Contracts.Search;

namespace Jcc.Commons.Platform.Application.Search;

public sealed record SearchQueryRequest(
    string[] HotelCodes,
    DateOnly CheckIn,
    DateOnly CheckOut,
    Occupancy Occupancy,
    string? Nationality = null,
    string? Currency = null,
    string? Language = null);
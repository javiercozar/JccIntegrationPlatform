namespace Jcc.Commons.Platform.Contracts.Search;

public sealed record SearchRequest(
    string[] HotelCodes,
    DateOnly CheckIn,
    DateOnly CheckOut,
    Occupancy Occupancy,
    string? Nationality = null,
    string? Currency = null,
    string? Language = null
);
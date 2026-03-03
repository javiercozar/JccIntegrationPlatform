using Jcc.Commons.Platform.Application.Search.Queries;
using Jcc.Commons.Platform.Grpc.Availability;

namespace Jcc.Commons.Platform.Application.Search.Mappers;

public static class SearchRequestExtensions
{
    public static SearchQueryRequest? MapToSearchQueryRequest(this SearchRequest? request)
    {
        if (request is null)
            return null;

        var hotelCodes = request.HotelIds?.ToArray() ?? [];

        return new SearchQueryRequest(
            hotelCodes, 
            CheckIn:ParseDate(request.Stay.CheckIn), 
            CheckOut:ParseDate(request.Stay.CheckOut), 
            Occupancy:null, 
            Nationality:Normalize(""), 
            Currency:Normalize(""), 
            Language:Normalize(string.Empty));

        static DateOnly? ParseDate(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return DateOnly.TryParse(text, out var dateOnly) 
                ? dateOnly 
                : null;
        }

        string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
    }
}

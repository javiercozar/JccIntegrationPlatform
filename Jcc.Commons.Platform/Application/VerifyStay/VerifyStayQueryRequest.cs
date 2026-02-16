namespace Jcc.Commons.Platform.Application.VerifyStay;

public sealed record VerifyStayQueryRequest(
    string HotelCode,
    string ReservationCode,
    DateTime CheckIn,
    DateTime CheckOut
);

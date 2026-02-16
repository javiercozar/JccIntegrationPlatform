namespace Jcc.Commons.Platform.Application.VerifyStay;

public sealed record VerifyStayQueryResponse(
    bool IsValid,
    string Message
);
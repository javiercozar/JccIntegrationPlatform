using Jcc.Commons.Platform.Domain;

namespace Jcc.Commons.Platform.Application.VerifyStay;

public sealed class VerifyStayQueryHandler : IQueryHandler<VerifyStayQueryRequest, VerifyStayQueryResponse> {
    public Task<VerifyStayQueryResponse> HandleAsync(VerifyStayQueryRequest request) {      
        return Task.FromResult(new VerifyStayQueryResponse(false, "Not implemented"));
    }
}

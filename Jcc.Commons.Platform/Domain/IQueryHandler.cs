namespace Jcc.Commons.Platform.Domain;

public interface IQueryHandler<TRequest, TResponse> {
    Task<TResponse> HandleAsync(TRequest request);
}
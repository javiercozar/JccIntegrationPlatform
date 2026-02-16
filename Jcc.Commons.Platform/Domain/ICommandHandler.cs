namespace Jcc.Commons.Platform.Domain;

public interface ICommandHandler<TRequest, TResponse> {
    Task<TResponse> HandleAsync(TRequest request);
}
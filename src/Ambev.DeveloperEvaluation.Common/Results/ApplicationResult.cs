using Ambev.DeveloperEvaluation.Common.Errors;

namespace Ambev.DeveloperEvaluation.Common.Results;

public class ApplicationResult<T> : IApplicationResult
{
    public T? Data { get; }
    public ApplicationError? Error { get; }

    private ApplicationResult(T data)
    {
        if (data is ApplicationError)
        {
            throw new ArgumentOutOfRangeException($"{typeof(T).Name} unsupported on {typeof(ApplicationResult<T>).Name} creation.");
        }
        Data = data;
    }

    private ApplicationResult(ApplicationError error)
    {
        Error = error;
    }

    private static ApplicationResult<T> Success(T data) => new(data);
    public static ApplicationResult<T> Failure(ApplicationError error) => new(error);

    public static implicit operator ApplicationResult<T> (T data) => Success(data);
    public static implicit operator ApplicationResult<T> (ApplicationError error) => Failure(error);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<ApplicationError, TResult> onFailure)
    {
        if (Data is not null)
            return onSuccess(Data);
        if (Error is not null)
            return onFailure(Error);

        // This should never occur due class design construction.
        throw new InvalidOperationException("Result is neither success nor failure");
    }

    public Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<ApplicationError, TResult> onFailure)
    {
        if (Data is not null)
            return onSuccess(Data);
        if (Error is not null)
            return Task.FromResult(onFailure(Error));

        // This should never occur due class design construction.
        throw new InvalidOperationException("Result is neither success nor failure");
    }
}

public interface IApplicationResult;

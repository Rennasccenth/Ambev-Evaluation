using Ambev.DeveloperEvaluation.Common.Errors;

namespace Ambev.DeveloperEvaluation.Common.Results;

public class CommandResult<T> : ICommandResult
{
    private T? Data { get; }
    private ApplicationError? Error { get; }

    private CommandResult(T data)
    {
        if (data is ApplicationError)
        {
            throw new ArgumentOutOfRangeException($"{typeof(T).Name} unsupported on {typeof(CommandResult<T>).Name} creation.");
        }
        Data = data;
    }

    private CommandResult(ApplicationError error)
    {
        Error = error;
    }

    private static CommandResult<T> Success(T data) => new(data);
    public static CommandResult<T> Failure(ApplicationError error) => new(error);

    public static implicit operator CommandResult<T> (T data) => Success(data);
    public static implicit operator CommandResult<T> (ApplicationError error) => Failure(error);

    /// <summary>
    /// Access the inner results of a <see cref="CommandResult{T}"/>
    /// <br/>
    /// <para>
    /// This function behaves by executing the <paramref name="onSuccess"/> or a Error <paramref name="onFailure"/>
    /// based on the inner value, which can be either a <see cref="T"/> in case of Success, or a <see cref="ApplicationError"/> in a error case.
    /// </para>
    /// </summary>
    /// <param name="onSuccess">Closure for a successful result.</param>
    /// <param name="onFailure">Closure for a failure result.</param>
    /// <typeparam name="TResult">Returning type of the closures.</typeparam>
    /// <returns>Specified <see cref="TResult"/>.</returns>
    /// <exception cref="InvalidOperationException">This exception should never occur based on the class design.</exception>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<ApplicationError, TResult> onFailure)
    {
        if (Data is not null)
            return onSuccess(Data);
        if (Error is not null)
            return onFailure(Error);

        // This should never occur due class design construction.
        throw new InvalidOperationException("CommandResult is neither success nor failure");
    }
}

public interface ICommandResult;
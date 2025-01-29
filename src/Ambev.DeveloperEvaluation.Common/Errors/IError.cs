namespace Ambev.DeveloperEvaluation.Common.Errors;

public interface IError
{
    public string Code { get; }
    public string Message { get; }
}

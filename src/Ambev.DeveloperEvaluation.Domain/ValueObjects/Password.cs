using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed partial class Password
{
    private readonly string _value;
    public bool IsValid { get; private set; }

    private Password(string value)
    {
        _value = value;
        IsValid = Validate();
    }

    public static implicit operator Password (string value) => new(value);
    public static implicit operator string (Password password) => password._value;

    private static readonly Regex UppercaseRegex = MyRegex();
    private static readonly Regex LowercaseRegex = MyRegex1();
    private static readonly Regex NumberRegex = MyRegex2();
    private static readonly Regex SpecialCharRegex = MyRegex3();

    internal List<string> ValidationErrors = [];
    private bool Validate()
    {
        if (!UppercaseRegex.IsMatch(_value))
        {
            ValidationErrors.Add("Password must contain at least one uppercase letter");
        }
        if (!LowercaseRegex.IsMatch(_value))
        {
            ValidationErrors.Add("Password must contain at least one lowercase letter");
        }
        if (!NumberRegex.IsMatch(_value))
        {
            ValidationErrors.Add("Password must contain at least one number");
        }
        if (!SpecialCharRegex.IsMatch(_value))
        {
            ValidationErrors.Add("Password must contain at least one special character");
        }
        switch (_value.Length)
        {
            case < 8:
                ValidationErrors.Add("Password must be at least 8 characters long");
                break;
            case > 20:
                ValidationErrors.Add("Password must be at most 20 characters long");
                break;
        }
        return ValidationErrors.Count == 0;
    }

    public override string ToString() => _value;

    [GeneratedRegex(@"[A-Z]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"[a-z]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex1();
    [GeneratedRegex(@"[0-9]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex2();
    [GeneratedRegex(@"[\!\?\*\.\@\#\$\%\^\&\+\=]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex3();
}
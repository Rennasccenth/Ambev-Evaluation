using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed partial class Phone
{
    private readonly string _value;

    public bool IsValid { get; private set; }

    private Phone(string value)
    {
        _value = value;
        IsValid = Validate();
    }

    public static implicit operator Phone(string value) => new(value);
    public static implicit operator string(Phone phone) => phone._value;
    
    public override string ToString() => _value;

    private bool Validate()
    {
        var regex = MyRegex();
        return regex.IsMatch(_value);
    }

    [GeneratedRegex(@"^\+(?!0)\d{7,14}$|^[1-9]\d{6,14}$")]
    private static partial Regex MyRegex();
}
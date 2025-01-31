namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed class Phone
{
    private readonly string _value;
    public const ushort MinLength = 10;
    public const ushort MaxLength = 20;

    public bool IsValid { get; private set; }

    private static readonly HashSet<char> AllowedCharacters =
    [
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '(', ')', '+', '-', ' '
    ];

    private Phone(string value)
    {
        _value = value;
        IsValid = Validate();
    }

    public static implicit operator Phone(string value) => new(value);
    public static implicit operator string (Phone phone) => phone._value;
    
    public override string ToString() => _value;

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(_value))
            return false;

        return _value.Length switch
        {
            < MinLength or > MaxLength => false,
            _ => _value.All(value => AllowedCharacters.Contains(value))
        };
    }
}
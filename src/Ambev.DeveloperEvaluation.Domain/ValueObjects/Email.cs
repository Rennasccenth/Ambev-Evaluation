using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed partial class Email
{
    private readonly string _address;
    public bool IsValid { get; private set; }

    private Email(string address)
    {
        _address = address;
        IsValid = Validate();
    }

    public static implicit operator Email(string address) => new(address);
    public static implicit operator string (Email email) => email._address;

    public override string ToString() => _address;

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(_address)) return false;
        if (_address.Length > 100) return false;
        return MyRegex().IsMatch(_address);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
}
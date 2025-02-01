namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Address
{
    public string City { get; init; } = null!;
    public string Street { get; init; } = null!;
    public int Number { get; init; }
    public string ZipCode { get; init; } = null!;
    public string Latitude { get; init; } = null!;
    public string Longitude { get; init; } = null!;

    // This internal ctor allows Address to be constructed by external entities like Mappers or ORM.
    internal Address() { }
    public Address(string city, string street, int number, string zipCode, string latitude, string longitude)
    {
        City = city;
        Street = street;
        Number = number;
        ZipCode = zipCode;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static implicit operator string(Address address) =>
        $"{address.Street} Num. {address.Number}, {address.ZipCode} - {address.City}";

    public override string ToString() =>
        $"{Street} Num. {Number}, {ZipCode} - {City}";
}
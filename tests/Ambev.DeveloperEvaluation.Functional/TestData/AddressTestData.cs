using System.Globalization;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.TestData;

public static class AddressTestData
{
    public static readonly Faker<Address> AddressFaker = new Faker<Address>()
        .CustomInstantiator(factoryMethod: faker => new Address(
            city: faker.Address.City(),
            street: faker.Address.StreetAddress(useFullAddress: true),
            number: int.Parse(s: faker.Address.BuildingNumber()),
            zipCode: faker.Address.ZipCode(),
            latitude: faker.Address.Latitude().ToString(provider: CultureInfo.InvariantCulture),
            longitude: faker.Address.Longitude().ToString(provider: CultureInfo.InvariantCulture)));
}
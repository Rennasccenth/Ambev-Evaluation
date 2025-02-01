using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.TestData;

internal static class ValueObjectGenerator
{
    private const string BrazilianLocale = "pt_BR";
    internal static class Password
    {
        private static readonly Faker Faker = new(BrazilianLocale);
        internal static string Generate()
        {
            return Faker.Internet.Password(prefix: "T3st!");
        }
    }

    internal static class Email
    {
        private static readonly Faker Faker = new(BrazilianLocale);
        internal static string Generate()
        {
            return Faker.Internet.Email();
        }
    }

    internal static class Phone
    {
        private static readonly Faker Faker = new(BrazilianLocale);
        internal static string Generate()
        {
            return Faker.Phone.PhoneNumber();
        }
    }
}
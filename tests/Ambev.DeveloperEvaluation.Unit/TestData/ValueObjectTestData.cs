using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.TestData
{
    public static class ValueObjectTestData
    {
        public static string GenerateValidEmail()
        {
            var faker = new Faker();
            return faker.Internet.ExampleEmail();
        }

        public static string GenerateInvalidEmail()
        {
            return "invalid-email-format";
        }

        public static string GenerateValidPhone()
        {
            var faker = new Faker();
            return $"+{faker.Random.Int(1, 9)}{faker.Random.Number(100000000, 999999999)}";
        }

        public static string GenerateValidPhoneWithoutPrefix()
        {
            var faker = new Faker();
            return faker.Random.Number(100000000, 999999999).ToString();
        }

        public static string GenerateInvalidPhone()
        {
            return "abc12345"; // Contains letters which are not allowed
        }
    }
}
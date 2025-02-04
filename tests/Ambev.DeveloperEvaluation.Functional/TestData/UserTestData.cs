using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Functional.TestCollections;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.TestData;

[Collection(PerCollectionFixture.CollectionName)]
public class UserTestData : ICollectionFixture<DeveloperEvaluationWebApplicationFactory>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<User> _userValidator;
    private readonly Faker _faker = new("pt_BR");

    public UserTestData(DeveloperEvaluationWebApplicationFactory webApiFixture)
    {
        IServiceScope serviceScope = webApiFixture.Services.CreateScope();
        _passwordHasher = serviceScope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        _timeProvider = serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
        _userValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<User>>();
    }

    private User.UserBuilder GetUserBuilder => User.GetBuilder(_passwordHasher, _timeProvider, _userValidator);

    internal string Email => _faker.Internet.Email(uniqueSuffix: _faker.UniqueIndex.ToString());
    internal string Password => _faker.Internet.Password(8, prefix: "!T3sT");

    private string Phone => $"+{_faker.Random.Int(1, 9)}{_faker.Random.Number(100000000, 999999999)}";

    /// <summary>
    /// Returns a user, without validating the properties.
    /// </summary>
    public User DumpUser(
        string? username = null,
        Password? password = null,
        string? firstname = null,
        string? lastname = null,
        Phone? phone = null,
        Email? email = null,
        Address? address = null,
        UserStatus? status = null,
        UserRole? role = null)
    {
        return GetUserBuilder
            .WithFirstname(firstname ?? _faker.Person.FirstName)
            .WithLastname(lastname ?? _faker.Person.LastName)
            .WithEmail(email ?? Email)
            .WithUsername(username ?? _faker.Person.UserName)
            .WithPassword(password ?? Password)
            .WithPhone(phone ?? Phone)
            .WithStatus(status ?? _faker.PickRandom<UserStatus>())
            .WithRole(role ?? _faker.PickRandom<UserRole>())
            .WithAddress(address ?? AddressTestData.AddressFaker)
            .Dump();
    }

    /// <summary>
    /// Returns a simple valid user.
    /// </summary>
    /// <exception cref="InvalidDataException"> In case the generated user don't get build,
    /// due missing test data configuration.</exception>
    public User ValidatedUser(
        string? username = null,
        Password? password = null,
        string? firstname = null,
        string? lastname = null,
        Phone? phone = null,
        Email? email = null,
        Address? address = null,
        UserStatus? status = null,
        UserRole? role = null)
    {
        var builderResult = GetUserBuilder
            .WithFirstname(firstname ?? _faker.Person.FirstName)
            .WithLastname(lastname ?? _faker.Person.LastName)
            .WithEmail(email ?? _faker.Person.Email)
            .WithUsername(username ?? _faker.Person.UserName)
            .WithPassword(password ?? Password)
            .WithPhone(phone ?? Phone)
            .WithStatus(status ?? _faker.PickRandom<UserStatus>())
            .WithRole(role ?? _faker.PickRandom<UserRole>())
            .WithAddress(address ?? AddressTestData.AddressFaker)
            .Build();

        return builderResult.IsT0 
            ? builderResult.AsT0 
            : throw new InvalidDataException($"{builderResult}. User test data creation seems to be miss configured.");
    }
}
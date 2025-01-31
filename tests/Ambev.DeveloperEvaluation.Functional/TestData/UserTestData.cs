using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.TestData;


/// <summary>
/// Generates User testing data by consuming the real services registered in DI container, avoiding de-sync registration problems.
/// </summary>
[Collection(TestHelperConstants.WebApiCollectionName)]
public class UserTestData
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

    private string Password => _passwordHasher.HashPassword(_faker.Internet.Password(8, prefix: "!T3sT"));

    private string Phone => "+" + string.Join("", Enumerable.Range(0, 13)
        .Select(_ => _faker.Random.Int(0, 9).ToString())
        .ToArray());

    public User BasicUser(UserStatus? status = null, UserRole? role = null)
    {
        var builderResult = GetUserBuilder
            .WithFirstname(_faker.Person.FirstName)
            .WithLastname(_faker.Person.LastName)
            .WithEmail(_faker.Person.Email)
            .WithUsername(_faker.Person.UserName)
            .WithPassword(Password)
            .WithPhone(Phone)
            .WithStatus(status ?? _faker.PickRandom<UserStatus>())
            .WithRole(role ?? _faker.PickRandom<UserRole>())
            .WithAddress(AddressTestData.AddressFaker)
            .Build();

        return builderResult.IsT0 
            ? builderResult.AsT0 
            : throw new InvalidDataException($"{builderResult}. User test data creation seems to be miss configured.");
    }

    public User BuildWithFaker(Faker<User> userFaker)
    {
        User fakeUser = userFaker
            .Generate();
        
        var builderResult = GetUserBuilder
            .WithFirstname(fakeUser.Firstname)
            .WithLastname(fakeUser.Lastname)
            .WithEmail(fakeUser.Email)
            .WithUsername(fakeUser.Username)
            .WithPassword(fakeUser.Password)
            .WithPhone(fakeUser.Phone)
            .WithStatus(fakeUser.Status)
            .WithRole(fakeUser.Role)
            .WithAddress(fakeUser.Address)
            .Build();

        return builderResult.IsT0 
            ? builderResult.AsT0 
            : throw new InvalidDataException($"Either {fakeUser} or {builderResult} are miss configured.");
    }
    //
    // public User ValidUser()
    // {
    //     User fakeUser = BaseFaker();
    //
    //     var builderResult = GetUserBuilder
    //         .WithFirstname(fakeUser.Firstname)
    //         .WithLastname(fakeUser.Lastname)
    //         .WithEmail(fakeUser.Email)
    //         .WithUsername(fakeUser.Username)
    //         .WithPassword(fakeUser.Password)
    //         .WithPhone(fakeUser.Phone)
    //         .WithStatus(fakeUser.Status)
    //         .WithRole(fakeUser.Role)
    //         .WithAddress(fakeUser.Address)
    //         .Build();
    //
    //     return builderResult.IsT0 
    //         ? builderResult.AsT0 
    //         : throw new InvalidDataException($"Either {fakeUser} or {builderResult} are miss configured.");
    // }
    //
    // public User ActiveUser()
    // {
    //     var activeUserFaker = BaseFaker()
    //         .RuleFor(usr => usr.Status, UserStatus.Active);
    //
    //     return BuildWithFaker(activeUserFaker);
    // }
    //
    // public User InactiveUser()
    // {
    //     var activeUserFaker = BaseFaker()
    //         .RuleFor(usr => usr.Status, UserStatus.Inactive);
    //
    //     return BuildWithFaker(activeUserFaker);
    // }
    //
    // public User UserWithId(Guid id)
    // {
    //     var activeUserFaker = BaseFaker()
    //         .RuleFor(usr => usr.Id, id);
    //
    //     return BuildWithFaker(activeUserFaker);
    // }
}
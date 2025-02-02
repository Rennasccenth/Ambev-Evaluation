using System.Collections;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Features;

public sealed class GetUsersTests : BaseTest
{
    private readonly IMediator _mediatR;
    private readonly Faker _faker = new("pt_BR");

    public GetUsersTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        _mediatR = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [Theory(DisplayName = "Get users when filter properties are provided, should return filtered users.")]
    [ClassData(typeof(GetUsersTestData))]
    public async Task GetUsers_ShouldFilterUsers_BasedOnGetUserQuery(
        uint userMatchingCount, 
        UserStatus? status,
        UserRole? role,
        string? username)
    {
        // Arrange

        // Generates multiple users matching the following criteria
        List<Task> creatingUsersTasks = [];
        for (var i = 1; i < userMatchingCount + 1; i++)
        {
            creatingUsersTasks.Add(TestUserBuilder.CreateUserAsync(
                userEntity: UserTestData.DumpUser(
                    status: status,
                    role: role,
                    username: username),
                httpClient: TestServerHttpClient));
        }

        await Task.WhenAll(creatingUsersTasks.ToArray());

        // Create another user with different data than provided.
        await TestUserBuilder.CreateUserAsync(
            userEntity: UserTestData.DumpUser(
                password: _faker.Internet.Password(length: 12, prefix: "T3s!T")
            ), httpClient: TestServerHttpClient);
        
        GetUsersQuery query = new GetUsersQuery
        {
            Page = 1,
            PageSize = 80,
            Status = status ?? null,
            Role = role ?? null,
            Username = username ?? null
        };

        // Act
        var activeUsersQueryApplicationResult = await _mediatR.Send(query);

        // Assert
        using (var _ = new AssertionScope())
        {
            activeUsersQueryApplicationResult.Error.Should().BeNull();
            activeUsersQueryApplicationResult.Data.Should().NotBeNull();
            GetUsersQueryResult queryResult = activeUsersQueryApplicationResult.Data!;

            queryResult.Page.Should().Be(query.Page);
            queryResult.PageSize.Should().Be(query.PageSize);
            queryResult.Users.Should().AllSatisfy(usrSummary =>
            {
                if (username is not null)
                    usrSummary.Username.Should().Contain(username);
                if (role is not null)
                    usrSummary.Role.Should().Be(role);
                if (status is not null)
                    usrSummary.Status.Should().Be(status);
            });
        }
    }

}

file sealed class GetUsersTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [15u, UserStatus.Active, null!, null!];
        yield return [20u, UserStatus.Active, UserRole.Customer, "johndoe"];
        yield return [9u,  UserStatus.Suspended, UserRole.Admin, "adminUser"];
        yield return [1u,  UserStatus.Inactive, null!, "guestUser"];
        yield return [1u,  UserStatus.Inactive, UserRole.Manager, "managerUser"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
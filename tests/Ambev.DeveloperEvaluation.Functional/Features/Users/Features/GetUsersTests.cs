using System.Collections;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Functional.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Features;

public sealed class GetUsersTests : BaseTest
{
    private readonly Faker _faker = new("pt_BR");

    public GetUsersTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
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

        // Create another users with different data than provided just to add randomness.
        await TestUserBuilder.CreateUserAsync(userEntity: UserTestData.DumpUser(
                password: _faker.Internet.Password(length: 12, prefix: "T3s!T")),
            httpClient: TestServerHttpClient);
        await TestUserBuilder.CreateUserAsync(userEntity: UserTestData.DumpUser(
                email: _faker.Internet.Email("randommin.com", uniqueSuffix: _faker.UniqueIndex.ToString())),
            httpClient: TestServerHttpClient);

        
        GetUsersRequest getUsersRequest = new GetUsersRequest
        {
            CurrentPage = 1,
            PageSize = 80,
            Status = status.ToString() ?? null,
            Role = role.ToString() ?? null,
            Username = username ?? null
        };

        // Act
        string queryStringObject = QueryStringHelper.ToQueryString(getUsersRequest);

        var getUsersResponse = await TestServerHttpClient.GetFromJsonAsync<GetUsersResponse>($"api/users{queryStringObject}");

        // Assert
        using (var _ = new AssertionScope())
        {
            getUsersResponse.Should().NotBeNull();
            getUsersResponse?.Data.Should().NotBeNull();

            if (getUsersResponse is not null)
            {
                getUsersResponse.PageNumber.Should().Be(getUsersRequest.CurrentPage);
                getUsersResponse.PageSize.Should().Be(getUsersRequest.PageSize);
                getUsersResponse.TotalCount.Should().Be((int)userMatchingCount);
                getUsersResponse.Data.Should().AllSatisfy(usrSummary =>
                {
                    if (username is not null)
                        usrSummary.Username.Should().Contain(username);
                    if (role is not null)
                        usrSummary.Role.Should().Be(role.ToString());
                    if (status is not null)
                        usrSummary.Status.Should().Be(status.ToString());
                });
            }
        }
    }

}

file sealed class GetUsersTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [20u, UserStatus.Active, UserRole.Customer, "johndoe"];
        yield return [9u,  UserStatus.Suspended, UserRole.Admin, "adminUser"];
        yield return [1u,  UserStatus.Inactive, UserRole.Manager, "managerUser"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Specifications;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Domain.Specifications;

public sealed class ActiveUserSpecificationTests : BaseTest
{
    public ActiveUserSpecificationTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory) { }

    [Theory]
    [InlineData(UserStatus.Active, true)]
    [InlineData(UserStatus.Inactive, false)]
    [InlineData(UserStatus.Suspended, false)]
    public void IsSatisfiedBy_ShouldValidateUserStatus(UserStatus status, bool expectedResult)
    {
        // Arrange
        ActiveUserSpecification specification = new();
        User testingUser = UserTestData.ValidatedUser(status: status);

        // Act
        var result = specification.IsSatisfiedBy(testingUser);

        // Assert
        result.Should().Be(expectedResult);
    }
}
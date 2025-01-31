using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Functional.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Users.Features.Specifications;

public sealed class ActiveUserSpecificationTests : BaseTest
{
    private readonly UserTestData _userTestData;

    public ActiveUserSpecificationTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        _userTestData = new UserTestData(webApplicationFactory);
    }

    [Theory]
    [InlineData(UserStatus.Active, true)]
    [InlineData(UserStatus.Inactive, false)]
    [InlineData(UserStatus.Suspended, false)]
    public void IsSatisfiedBy_ShouldValidateUserStatus(UserStatus status, bool expectedResult)
    {
        // Arrange
        ActiveUserSpecification specification = new();
        User testingUser = _userTestData.BasicUser(status);

        // Act
        var result = specification.IsSatisfiedBy(testingUser);

        // Assert
        result.Should().Be(expectedResult);
    }
}
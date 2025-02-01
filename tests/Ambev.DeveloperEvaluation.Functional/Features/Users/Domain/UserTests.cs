using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Functional.TestData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Domain;

public sealed class UserTests : BaseTest
{
    private readonly UserTestData _userTestData;
    private readonly FakeTimeProvider _currentTimeProvider;
    
    public UserTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        _currentTimeProvider = (FakeTimeProvider)serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
        _userTestData = new UserTestData(webApplicationFactory);
    }

    [Fact(DisplayName = "User status should change to Active when Activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        User user = _userTestData.ValidatedUser();

        // Act
        user.Activate(_currentTimeProvider);

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is suspended, their status changes to Suspended.
    /// </summary>
    [Fact(DisplayName = "User status should change to Suspended when Suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        // Arrange
        User user = _userTestData.ValidatedUser();

        // Act
        user.Suspend(_currentTimeProvider);

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    [Fact(DisplayName = "User status should change to Inactive when Deactivated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeDeactivated()
    {
        // Arrange
        User user = _userTestData.ValidatedUser();

        // Act
        user.Deactivate(_currentTimeProvider);

        // Assert
        Assert.Equal(UserStatus.Inactive, user.Status);
    }
}
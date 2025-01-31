using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Time.Testing;
using OneOf;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
public class UserTests
{
    private static readonly TimeProvider TimeProvider = new FakeTimeProvider();

    /// <summary>
    /// Tests that when a suspended user is activated, their status changes to Active.
    /// </summary>
    [Fact(DisplayName = "User status should change to Active when activated")]
    public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
    {
        // Arrange
        User user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Suspended;

        // Act
        user.Activate(TimeProvider);

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    /// <summary>
    /// Tests that when an active user is suspended, their status changes to Suspended.
    /// </summary>
    [Fact(DisplayName = "User status should change to Suspended when suspended")]
    public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
    {
        // Arrange
        User user = UserTestData.GenerateValidUser();
        user.Status = UserStatus.Active;

        // Act
        user.Suspend(TimeProvider);

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    // /// <summary>
    // /// Tests that validation passes when all user properties are valid.
    // /// </summary>
    // [Fact(DisplayName = "Validation should pass for valid user data")]
    // public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
    // {
    //     // Arrange
    //     User user = UserTestData.GenerateValidUser();
    //
    //     // Act
    //     ValidationResultDetail result = user.Validate();
    //
    //     // Assert
    //     Assert.True(result.IsValid);
    //     Assert.Empty(result.Errors);
    // }

    /// <summary>
    /// Tests that user build passes when all user properties are valid.
    /// </summary>
    [Fact(DisplayName = "User build should be able to build when data is valid.")]
    public void Given_ValidUserData_When_Build_Then_ShouldReturnUser()
    {
        // Arrange
        User validUser = UserTestData.GenerateValidUser();

        // We shoudl be able to inject these when move tests to AppFactory
        IPasswordHasher passwordHasher = new BCryptPasswordHasher();
        IValidator<User> userValidator = new UserValidator();

        // Act
        OneOf<User, ValidationResult> buildResult = User.GetBuilder(passwordHasher, TimeProvider, userValidator)
            .WithUsername(validUser.Username)
            .WithPassword(validUser.Password)
            .WithEmail(validUser.Email)
            .WithFirstname(validUser.Firstname)
            .WithLastname(validUser.Lastname)
            .WithAddress(new Address(
                validUser.Address.City,
                validUser.Address.Street,
                validUser.Address.Number,
                validUser.Address.ZipCode,
                validUser.Address.Latitude,
                validUser.Address.Longitude))
            .WithPhone(validUser.Phone)
            .WithStatus(validUser.Status)
            .WithRole(validUser.Role)
            .Build();

        // Assert
        buildResult.Should().BeOfType<User>("all provided data is valid.");
    }

    // [Fact(DisplayName = "Validation should fail for invalid user data")]
    // public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
    // {
    //     // Arrange
    //     var user = new User(username: "", // Invalid: empty
    //         password: UserTestData.GenerateInvalidPassword(), // Invalid: doesn't meet password requirements
    //         email: UserTestData.GenerateInvalidEmail(), // Invalid: not a valid email
    //         phone: UserTestData.GenerateInvalidPhone(), // Invalid: doesn't match pattern
    //         status: UserStatus.Unknown, // Invalid: cannot be Unknown
    //         role: UserRole.None // Invalid: cannot be None
    //     );
    //
    //     // Act
    //     ValidationResultDetail result = user.Validate();
    //
    //     // Assert
    //     Assert.False(result.IsValid);
    //     Assert.NotEmpty(result.Errors);
    // }
}

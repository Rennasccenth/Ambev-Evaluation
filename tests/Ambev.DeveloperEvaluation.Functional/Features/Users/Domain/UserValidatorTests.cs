using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Domain;

public sealed class UserValidatorTests : BaseTest
{
    private readonly IValidator<User> _userValidator;

    public UserValidatorTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory)
    {
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        _userValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<User>>();
    }

    [Fact(DisplayName = "Valid user should pass all validation rules")]
    public void Given_ValidUser_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        User validUser = UserTestData.DumpUser();

        // Act
        var result = _userValidator.TestValidate(validUser);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "Invalid username formats should fail validation")]
    [InlineData("")]     // Empty
    [InlineData("ab")]   // Too short
    [InlineData("a")]    // Too short
    public void Given_InvalidUsername_When_Validated_Then_ShouldHaveError(string username)
    {
        // Arrange
        User userResult = UserTestData.DumpUser(username: username);

        // Act
        var result = _userValidator.TestValidate(userResult);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Theory(DisplayName = "Invalid email formats should fail validation")]
    [InlineData("invalid-email")]
    [InlineData("")]
    public void Given_InvalidEmail_When_Validated_Then_ShouldHaveError(string email)
    {
        // Arrange
        User userResult = UserTestData.DumpUser(email: email);

        // Act
        var result = _userValidator.TestValidate(userResult);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory(DisplayName = "Invalid password formats should fail validation")]
    [InlineData("")]     // Empty
    [InlineData("abDADSD")]   // Too short
    [InlineData("a")]    // Too short
    [InlineData("aASDaa@sdasd!")]    // Doesn't contains numbers
    [InlineData("sdaa891a1sDAd")]    // Doesn't special characters
    public void Given_InvalidPassword_When_Validated_Then_ShouldHaveError(string password)
    {
        // Arrange
        User userResult = UserTestData.DumpUser(password: password);

        // Act
        var result = _userValidator.TestValidate(userResult);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
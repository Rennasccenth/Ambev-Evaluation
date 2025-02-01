using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.TestData;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class EmailValidatorTests
{
    private static readonly IValidator<Email> EmailValidator = new EmailValidator();

    [Fact(DisplayName = "Valid email formats should pass validation")]
    public void Given_ValidEmailFormat_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        Email email = ValueObjectTestData.GenerateValidEmail();

        // Act
        var result = EmailValidator.TestValidate(email);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Empty email should fail validation")]
    public void Given_EmptyEmail_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        Email emptyEmail = string.Empty;

        // Act
        var result = EmailValidator.TestValidate(emptyEmail);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Theory(DisplayName = "Invalid email formats should fail validation")]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain.")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldHaveError(Email email)
    {
        // Act
        var result = EmailValidator.TestValidate(email);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact(DisplayName = "Email exceeding maximum length should fail validation")]
    public void Given_EmailExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        Email email = $"{"a".PadLeft(95, 'a')}@example.com"; // Creates email > 100 chars

        // Act
        var result = EmailValidator.TestValidate(email);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}

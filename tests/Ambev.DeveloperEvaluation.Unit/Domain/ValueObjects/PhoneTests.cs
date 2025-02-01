using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.ValueObjects;

public class PhoneTests
{
    [Theory(DisplayName = "Phone number validation should work correctly for Phone VO")]
    [InlineData("+123456789", true)]         // Valid with plus
    [InlineData("123456789", true)]          // Valid without plus
    [InlineData("+5511999999999", true)]     // Valid Brazilian format
    [InlineData("11999999999", true)]        // Valid Brazilian format without plus
    [InlineData("abc123", false)]            // Invalid - contains letters
    [InlineData("", false)]                  // Invalid - empty
    [InlineData("+", false)]                 // Invalid - only plus
    [InlineData("123", false)]               // Invalid - too short
    [InlineData("+123456789012345678", false)] // Invalid - too long
    [InlineData("12.345-678", false)]        // Invalid - contains dot
    public void TestPhoneValidation(string phoneNumber, bool expectedIsValid)
    {
        // Arrange
        Phone phone = phoneNumber;

        // Assert
        Assert.Equal(expectedIsValid, phone.IsValid);
    }
}
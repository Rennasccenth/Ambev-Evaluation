using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Domain;

public sealed class UserBuilderTests : BaseTest
{
    private readonly User.UserBuilder _userBuilder;

    public UserBuilderTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        var passwordHasher = serviceScope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var userValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<User>>();
        _userBuilder = User.GetBuilder(passwordHasher, CurrentTimeProvider, userValidator);
    }
    
    
    [Theory(DisplayName = "User build should be able to build when data is valid.")]
    [InlineData(
        "john.doe", "StrongPass123!", "john.doe@email.com", 
        "John", "Doe", "New York", "Broadway", 123, 
        "10001", "40.7128", "-74.0060", "+1234567890",
        UserStatus.Active, UserRole.Manager)]
    [InlineData(
        "jane.smith", "SecurePass456!", "jane.smith@email.com",
        "Jane", "Smith", "Los Angeles", "Sunset Blvd", 456,
        "90028", "34.0522", "-118.2437", "+1987654321",
        UserStatus.Active, UserRole.Customer)]
    [InlineData(
        "bob.wilson", "Pass789Secure!", "bob.wilson@email.com",
        "Bob", "Wilson", "Chicago", "Michigan Ave", 789,
        "60601", "41.8781", "-87.6298", "+1122334455",
        UserStatus.Active, UserRole.Admin)]
    [InlineData(
        "alice.johnson", "P@ssw0rd123", "alice.johnson@email.com",
        "Alice", "Johnson", "Miami", "Ocean Drive", 321,
        "33139", "25.7617", "-80.1918", "+1445566778",
        UserStatus.Active, UserRole.Customer)]
    [InlineData(
        "david.brown", "Br0wnP@ss!", "david.brown@email.com",
        "David", "Brown", "Seattle", "Pike Street", 555,
        "98101", "47.6062", "-122.3321", "+1998877665",
        UserStatus.Active, UserRole.Admin)]
    public void Given_ValidUserData_When_Build_Then_ShouldReturnUser(string username, Password password,
        Email email, string firstname, string lastname, string city, string street, int number,
        string zipCode, string latitude, string longitude, Phone phone, UserStatus status, UserRole role)
    {
        // Arrange
        Address address = new(city, street, number, zipCode, latitude, longitude);

        // Act
        var buildResult = _userBuilder
            .WithUsername(username)
            .WithPassword(password)
            .WithEmail(email)
            .WithFirstname(firstname)
            .WithLastname(lastname)
            .WithAddress(address)
            .WithPhone(phone)
            .WithStatus(status)
            .WithRole(role)
            .Build();

        // Assert
        buildResult.IsT0.Should().BeTrue("all provided data is correct so we should retrieve a User.");
    }
}

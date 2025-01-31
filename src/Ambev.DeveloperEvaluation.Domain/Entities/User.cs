using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public partial class User : BaseEntity
{
    // The only way to build User entities is going by Builder, unless you use some internal scan
    internal User() { }

    public string Username { get; internal set; } = null!;
    public string Email { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static UserBuilder GetBuilder(
        IPasswordHasher passwordHasher,
        TimeProvider timeProvider,
        IValidator<User> userValidator)
        => new (passwordHasher, timeProvider, userValidator);

    /// <summary>
    /// Fluent builder pattern for User, intended to build User entities while keep them valid.
    /// </summary>
    public sealed class UserBuilder
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly TimeProvider _timeProvider;
        private readonly IValidator<User> _userValidator;
        private readonly User _buildingUser;

        internal UserBuilder(
            IPasswordHasher passwordHasher,
            TimeProvider timeProvider,
            IValidator<User> userValidator)
        {
            _passwordHasher = passwordHasher;
            _timeProvider = timeProvider;
            _userValidator = userValidator;
            _buildingUser = new User();
        }
        public UserBuilder WithEmail(string email)
        {
            _buildingUser.Email = email;
            return this;
        }
        public UserBuilder WithUsername(string username)
        {
            _buildingUser.Username = username;
            return this;
        }
        public UserBuilder WithPassword(string password)
        {
            _buildingUser.Password = _passwordHasher.HashPassword(password);
            return this;
        }
        public UserBuilder WithFirstname(string firstname)
        {
            _buildingUser.Firstname = firstname;
            return this;
        }
        public UserBuilder WithLastname(string lastname)
        {
            _buildingUser.Lastname = lastname;
            return this;
        }
        public UserBuilder WithAddress(Address address)
        {
            _buildingUser.Address = address;
            return this;
        }
        public UserBuilder WithStatus(UserStatus status)
        {
            _buildingUser.Status = status;
            return this;
        }
        public UserBuilder WithPhone(string phone)
        {
            _buildingUser.Phone = phone;
            return this;
        }
        public UserBuilder WithRole(UserRole role)
        {
            _buildingUser.Role = role;
            return this;
        }

        public OneOf<User, ValidationResult> Build()
        {
            _buildingUser.CreatedAt = _timeProvider.GetUtcNow().DateTime;

            ValidationResult? validationResult = _userValidator.Validate(_buildingUser);
            if (validationResult.IsValid) return _buildingUser;

            return validationResult;
        }
    }

    public void Activate(TimeProvider timeProvider)
    {
        Status = UserStatus.Active;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
    }

    public void Deactivate(TimeProvider timeProvider)
    {
        Status = UserStatus.Inactive;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
    }

    public void Suspend(TimeProvider timeProvider)
    {
        Status = UserStatus.Suspended;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
    }
}

public partial class User : IUser
{
    string IUser.Id => Id.ToString();
    string IUser.Username => Username;
    string IUser.Role => Role.ToString();
}
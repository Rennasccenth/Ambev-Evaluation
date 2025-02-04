using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.Users;
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

    public string Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;
    public string Firstname { get; private set; } = null!;
    public string Lastname { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static UserBuilder GetBuilder(
        IPasswordHasher passwordHasher,
        TimeProvider timeProvider,
        IValidator<User> userValidator)
        => new (passwordHasher, timeProvider, userValidator);

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
    public User UpdateUsername(string username, TimeProvider timeProvider)
    {
        if (Username == username) return this;
        Username = username;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User ChangePassword(Password newPassword, IPasswordHasher passwordHasher, TimeProvider timeProvider)
    {
        var newHashedPassword = passwordHasher.HashPassword(newPassword);
        if (Password == newHashedPassword) return this;
        Password = newHashedPassword;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User UpdateAddress(Address newAddress, TimeProvider timeProvider)
    {
        if (Address == newAddress) return this;
        Address = newAddress;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User UpdatePhone(Phone newPhone, TimeProvider timeProvider)
    {
        if (Phone == newPhone) return this;
        Phone = newPhone;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User UpdateEmail(Email newEmail, TimeProvider timeProvider)
    {
        if (Email == newEmail) return this;
        Email = newEmail;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User UpdateRole(UserRole newRole, TimeProvider timeProvider)
    {
        if (Role == newRole) return this;
        Role = newRole;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }
    public User UpdateStatus(UserStatus newStatus, TimeProvider timeProvider)
    {
        if (Status == newStatus) return this;
        Status = newStatus;
        UpdatedAt = timeProvider.GetUtcNow().DateTime;
        return this;
    }

    /// <summary>
    /// Fluent builder pattern for User, intended to build User entities while keep them valid.
    /// </summary>
    public sealed class UserBuilder
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly TimeProvider _timeProvider;
        private readonly IValidator<User> _userValidator;
        private readonly User _buildingUser;
        private string HashedPassword { get; set; } = "";

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
        public UserBuilder WithEmail(Email email)
        {
            _buildingUser.Email = email;
            return this;
        }
        public UserBuilder WithUsername(string username)
        {
            _buildingUser.Username = username;
            return this;
        }
        public UserBuilder WithPassword(Password password)
        {
            HashedPassword = _passwordHasher.HashPassword(password);
            _buildingUser.Password = password;
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
        public UserBuilder WithPhone(Phone phone)
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
            if (!validationResult.IsValid) return validationResult;

            // Only sets the hashed password once the creating object was validated
            // because otherwise we will try to validate a hashed password.
            _buildingUser.Password = HashedPassword;
            _buildingUser.AddDomainEvent(UserRegisteredEvent.From(_buildingUser, _timeProvider));
            return _buildingUser;
        }

        /// <summary>
        /// Dumps the internal building <see cref="User"/> instance, without any validation. 
        /// </summary>
        /// <returns>Unvalidated <see cref="User"/></returns>
        public User Dump()
        {
            return _buildingUser;
        }
    }
}

public partial class User : IUser
{
    string IUser.Id => Id.ToString();
    string IUser.Username => Username;
    string IUser.Role => Role.ToString();
}
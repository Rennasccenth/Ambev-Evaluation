using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public sealed class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(email => email)
            .Must(email => email.IsValid)
            .WithMessage("Invalid email address.");
    }
}

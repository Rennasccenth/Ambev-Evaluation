using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Configure JwtSettings for Options usage
/// </summary>
public sealed record JwtSettings
{
    public const string SectionName = "Jwt";

    [Required(ErrorMessage = $"A SecretKey must be provided in {SectionName}__{nameof(SecretKey)} environment variable.")]
    [MinLength(32, ErrorMessage = "The SecretKey must be at least 32 bytes length.")]
    public required string SecretKey { get; init; }

    [Required(ErrorMessage = $"An Issuer must be provided in {SectionName}__{nameof(Issuer)} environment variable.")]
    public required string Issuer { get; init; }

    [Required(ErrorMessage = $"An Audience must be provided in {SectionName}__{nameof(Audiences)} environment variable.")]
    [MinLength(1, ErrorMessage = $"At least one Audience must be provided in {SectionName}__{nameof(Audiences)} environment variable.")]
    public required string[] Audiences { get; init; }

    [Required(ErrorMessage = $"An ExpirationMinutes must be provided in {SectionName}__{nameof(ExpirationMinutes)} environment variable.")]
    public int ExpirationMinutes { get; init; } = 60;
}
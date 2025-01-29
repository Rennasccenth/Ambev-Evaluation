using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Configure JwtSettings for Options usage
/// </summary>
public sealed record JwtSettings
{
    public const string SectionName = "Jwt";

    [Required(ErrorMessage = $"A SecretKey must be provided inside {SectionName} section")]
    [MinLength(32, ErrorMessage = "The SecretKey must be at least 32 bytes length.")]
    public required string SecretKey { get; init; }

    public bool RequireMetadata { get; init; } = true;
    public bool SaveToken { get; init; } = true;
}
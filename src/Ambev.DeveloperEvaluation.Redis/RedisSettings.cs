using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Redis;

public sealed class RedisSettings
{
    public const string SectionName = "Redis";

    [Required(ErrorMessage = $"The application requires a Redis instance Host Address on {SectionName}__{nameof(HostAddress)} variable.")]
    public required string HostAddress { get; set; }

    [Required(ErrorMessage = $"The application requires a Redis instance password on {SectionName}__{nameof(Password)} variable.")]
    public required string Password { get; set; }

    [Required(ErrorMessage = $"The application requires a Redis retry delay in ms on {SectionName}__{nameof(RetryDelayInMilliseconds)} variable.")]
    public required uint RetryDelayInMilliseconds { get; init; } = 3;

    [Required(ErrorMessage = $"The application requires a Redis command timeout on {SectionName}__{nameof(CommandTimeoutInSeconds)} variable.")]
    public required uint CommandTimeoutInSeconds { get; init; } = 10;
    
    [Required(ErrorMessage = $"The application requires a Redis sync timeout on {SectionName}__{nameof(SyncTimeout)} variable.")]
    public required uint SyncTimeout { get; init; } = 60;
}

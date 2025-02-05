using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Redis;

public sealed class RedisSettings
{
    public const string SectionName = "Redis";

    [Required(ErrorMessage = $"The application requires a Redis instance conn string on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required string ConnectionString { get; set; }

    [Required(ErrorMessage = $"The application requires a Redis retry delay in ms on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint RetryDelayInMilliseconds { get; init; } = 3;

    [Required(ErrorMessage = $"The application requires a Redis command timeout on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint CommandTimeoutInSeconds { get; init; } = 10;
    
    [Required(ErrorMessage = $"The application requires a Redis sync timeout on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint SyncTimeout { get; init; } = 60;
}

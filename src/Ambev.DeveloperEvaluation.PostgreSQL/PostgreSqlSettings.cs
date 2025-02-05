using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.ORM;

public sealed class PostgreSqlSettings
{
    public const string SectionName = "PostgreSQL";

    [Required(ErrorMessage = $"The application requires a PostgreSQL database connection configuration on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required string ConnectionString { get; init; }
    
    [Required(ErrorMessage = $"The application requires a PostgreSQL max retry count configuration on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint MaxRetryCount { get; init; } = 3;
    
    [Required(ErrorMessage = $"The application requires a PostgreSQL retry delay in seconds configuration on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint RetryDelayInSeconds { get; init; } = 3;

    
    [Required(ErrorMessage = $"The application requires a PostgreSQL command timeout on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required uint CommandTimeoutInSeconds { get; init; } = 60;

    public required bool EnableDetailedErrors { get; init; } = false;

    public required bool EnableSensitiveDataLogging { get; init; } = false;
}
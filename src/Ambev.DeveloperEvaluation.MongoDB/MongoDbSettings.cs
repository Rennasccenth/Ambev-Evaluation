using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.MongoDB;

public sealed class MongoDbSettings
{
    public const string SectionName = "MongoDB";

    [Required(ErrorMessage = $"The application requires a Mongo DB instance on {SectionName}__{nameof(ConnectionString)} variable.")]
    public required string ConnectionString { get; set; }

    [Required(ErrorMessage = $"The application requires a Mongo DB Database Name under the {SectionName}__{nameof(DatabaseName)} variable.")]
    public required string DatabaseName { get; set; }
}
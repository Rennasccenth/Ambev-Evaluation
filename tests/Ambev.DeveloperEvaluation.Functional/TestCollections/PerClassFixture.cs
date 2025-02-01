using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.TestCollections;

[CollectionDefinition(CollectionName)]
public class PerClassFixture : IClassFixture<DeveloperEvaluationWebApplicationFactory>
{
    public const string CollectionName = nameof(PerClassFixture);
}
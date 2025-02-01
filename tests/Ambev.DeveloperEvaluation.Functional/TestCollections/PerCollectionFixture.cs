using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.TestCollections;

[CollectionDefinition(CollectionName)]
public class PerCollectionFixture : ICollectionFixture<DeveloperEvaluationWebApplicationFactory>
{
    public const string CollectionName = nameof(PerCollectionFixture);
}

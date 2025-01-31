using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

[CollectionDefinition(TestHelperConstants.WebApiCollectionName)]
public sealed class SharedTestCollection : ICollectionFixture<DeveloperEvaluationWebApplicationFactory>;

internal static class TestHelperConstants
{
    /// <summary>
    /// Design
    /// </summary>
    internal const string WebApiCollectionName = "WebApiCollection";
}
// using Ambev.DeveloperEvaluation.Functional.TestCollections;
// using Ambev.DeveloperEvaluation.Functional.TestData;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Time.Testing;
// using Xunit;
//
// namespace Ambev.DeveloperEvaluation.Functional;
//
// [Collection(PerClassFixture.CollectionName)]
// public class PersistentTest
// {
//     protected readonly UserTestData UserTestData;
//     protected readonly HttpClient TestServerHttpClient;
//     protected readonly FakeTimeProvider CurrentTimeProvider;
//
//     // This one will not try to reset the database by calling respawn.
//     protected PersistentTest(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
//     {
//         TestServerHttpClient = webApplicationFactory.CreateClient();
//
//         IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
//         CurrentTimeProvider = (FakeTimeProvider) serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
//         UserTestData = new UserTestData(webApplicationFactory);
//     }
// }
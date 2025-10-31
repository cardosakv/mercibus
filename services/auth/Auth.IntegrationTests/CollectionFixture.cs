namespace Auth.IntegrationTests;

/// <summary>
/// Collection definition for sequential test execution with shared test web app factory.
/// </summary>
[CollectionDefinition("Integration Tests Collection", DisableParallelization = true)]
public class IntegrationTestsCollection : ICollectionFixture<TestWebAppFactory>
{
}
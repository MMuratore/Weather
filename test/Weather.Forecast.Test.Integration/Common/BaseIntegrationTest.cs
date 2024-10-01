using FastEndpoints.Testing;

namespace Weather.Forecast.Test.Integration.Common;

[Collection(ApiFactoryCollection.Name)]
public abstract class BaseIntegrationTest(ApiFactory app) : TestBase
{
    protected readonly ApiFactory App = app;

    protected override async Task TearDownAsync()
    {
        App.Client.DefaultRequestHeaders.Remove(MockAuthenticationHandler.Connected);
        App.Client.DefaultRequestHeaders.Remove(MockAuthenticationHandler.UserId);
        App.Client.DefaultRequestHeaders.Remove(MockAuthenticationHandler.UserRole);
        await App.DatabaseFixture.ResetDatabaseAsync();
    }
}

[CollectionDefinition(Name)]
public sealed class ApiFactoryCollection : TestCollection<ApiFactory>
{
    public const string Name = nameof(ApiFactoryCollection);
}
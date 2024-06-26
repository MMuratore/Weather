namespace Weather.Forecast.Integration.Test.Common;

[Collection(ApiFactoryCollection.Name)]
public abstract class BaseIntegrationTest(ApiFactory app) : IAsyncLifetime
{
    protected readonly ApiFactory App = app;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => App.DatabaseFixture.ResetDatabaseAsync();
}


[CollectionDefinition(Name)]
public sealed class ApiFactoryCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = nameof(ApiFactoryCollection);
}
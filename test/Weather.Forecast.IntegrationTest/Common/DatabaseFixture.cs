using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace Weather.Forecast.Integration.Test.Common;

internal sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container =
        new PostgreSqlBuilder().WithImage("postgres:latest").Build();

    private Respawner? _respawn;

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        ConnectionString = container.GetConnectionString();
    }

    public Task DisposeAsync()
        => container.DisposeAsync().AsTask();

    public async Task ResetDatabaseAsync()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        _respawn ??= await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            TablesToIgnore =
            [
                new Table("__EFMigrationsHistory")
            ],
            DbAdapter = DbAdapter.Postgres
        });

        await _respawn.ResetAsync(connection);
    }
}
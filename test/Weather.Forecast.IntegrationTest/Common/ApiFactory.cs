using System.Net.Http.Headers;
using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Weather.Forecast.Features.Forecasts;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Integration.Test.Common;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    internal readonly DatabaseFixture DatabaseFixture = new();
    public readonly Faker Faker = new();
    public HttpClient Client { get; set; } = null!;

    internal ForecastDbContext CreateDbContext() =>
        new(new DbContextOptionsBuilder<ForecastDbContext>()
            .UseNpgsql(DatabaseFixture.ConnectionString)
            .Options);
    
    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(MockAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task InitializeAsync()
    {
        await DatabaseFixture.InitializeAsync();
        Client = CreateClient();
    }

    public new async Task DisposeAsync() => await DatabaseFixture.DisposeAsync();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureAuthenticationServices(services);
            ConfigureDatabaseServices(services);
        });
    }

    private void ConfigureDatabaseServices(IServiceCollection services)
    {
        services.RemoveHostedService<ForecastSeederHostedService>();
        services.RemoveHostedService<QuartzHostedService>();

        services.RemoveAll(typeof(ForecastDbContext));
        services.RemoveAll(typeof(DbContextOptions<ForecastDbContext>));
        services.AddDbContext<ForecastDbContext>(options =>
        {
            options.UseNpgsql(DatabaseFixture.ConnectionString);
        });
        
        services.AddQuartz(o =>
        {
            o.UsePersistentStore(c =>
            {
                c.UsePostgres(p =>
                {
                    p.ConnectionString =  DatabaseFixture.ConnectionString;
                    p.TablePrefix = "quartz.";
                });
                c.UseNewtonsoftJsonSerializer();
            });
        });
        
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
        context.Database.Migrate();
        
        var file = File.ReadAllText("../../../../.././.local/.postgres/init.sql");
        context.Database.ExecuteSqlRaw(file);
    }
    
    private static void ConfigureAuthenticationServices(IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = MockAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = MockAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = MockAuthenticationDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(
                MockAuthenticationDefaults.AuthenticationScheme, _ => { });
    }
}
﻿using System.Net.Http.Headers;
using FastEndpoints.Testing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Weather.Forecast.Features.Forecasts;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Integration.Test.Common;

public class ApiFactory : AppFixture<Program>
{
    internal readonly DatabaseFixture DatabaseFixture = new();

    internal ForecastDbContext CreateDbContext() =>
        new(new DbContextOptionsBuilder<ForecastDbContext>()
            .UseNpgsql(DatabaseFixture.ConnectionString)
            .Options);

    protected override Task SetupAsync()
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(MockAuthenticationDefaults.AuthenticationScheme);
        return Task.CompletedTask;
    }

    protected override async Task PreSetupAsync() => await DatabaseFixture.InitializeAsync();

    protected override async Task TearDownAsync() => await DatabaseFixture.DisposeAsync();

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        a.UseSetting("ConnectionStrings:Default", DatabaseFixture.ConnectionString);
        a.UseSetting("ConnectionStrings:Forecast", DatabaseFixture.ConnectionString);
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.RemoveHostedService<ForecastSeederHostedService>();
        s.RemoveHostedService<QuartzHostedService>();
        ConfigureMockAuthenticationServices(s);
        MigrateDatabase(s);
    }


    private static void ConfigureMockAuthenticationServices(IServiceCollection services)
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

    private static void MigrateDatabase(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
        context.Database.Migrate();

        var file = File.ReadAllText("../../../../.././.local/.postgres/init.sql");
        context.Database.ExecuteSqlRaw(file);
    }
}
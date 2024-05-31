using System.Reflection;
using Weather.Api.Configuration.FastEndpoint;
using Weather.Api.Configuration.HealthCheck;
using Weather.Api.Configuration.Localization;
using Weather.Api.Configuration.Observability;
using Weather.Forecast;
using Weather.Notification;
using Weather.SharedKernel.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddObservability();
builder.AddHealthChecks();
builder.AddFastEndpoint();
builder.AddLocalization();

List<Assembly> moduleAssemblies = [typeof(Program).Assembly];

builder.AddForecastModule(moduleAssemblies);
builder.AddNotificationModule(moduleAssemblies);
builder.AddTransactionalDispatcher(moduleAssemblies);

var app = builder.Build();

app.UseRequestLocalization();
app.UseHealthChecks();
app.UseFastEndpoint();

app.Run();
using System.Reflection;
using FastEndpoints;
using Weather.Api.Configuration.FastEndpoint;
using Weather.Api.Configuration.HealthCheck;
using Weather.Api.Configuration.Localization;
using Weather.Api.Configuration.Observability;
using Weather.Api.Configuration.Security;
using Weather.Forecast;
using Weather.Notification;
using Weather.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.AddObservability();
builder.AddHealthChecks();
builder.AddAuthentication();
builder.AddAuthorization();
builder.AddFastEndpoint();
builder.AddLocalization();

List<Assembly> moduleAssemblies = [typeof(Weather.Api.Program).Assembly];

builder.AddForecastModule(moduleAssemblies);
builder.AddNotificationModule(moduleAssemblies);
builder.AddMediatR(moduleAssemblies);

var app = builder.Build();

app.UseDefaultExceptionHandler();
app.UseRouting();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks();
app.UseFastEndpoint();

app.Run();


namespace Weather.Api
{
    public class Program;
}
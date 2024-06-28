using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;

namespace Weather.Forecast.Common.HttpClient;

internal sealed class OpenWeatherMapClient(
    System.Net.Http.HttpClient client,
    ForecastDbContext dbContext,
    IOptions<OpenWeatherMapOptions> options)
{
    private readonly OpenWeatherMapOptions Options = options.Value;

    public async Task<WeatherForecast?> GetForecastFrom(string city)
    {
        var response = await client.GetFromJsonAsync<OpenWeatherForecastResponse>(
            $"weather?q={city}&units=metric&appid={Options.ApiKey}");

        if (response is null) return null;

        var date = DateOnly.FromDateTime(DateTime.UnixEpoch.AddSeconds(response.dt));
        var forecast = WeatherForecast.Create(date, new Temperature((decimal)response.main.temp));
        await dbContext.Set<WeatherForecast>().AddAsync(forecast);
        await dbContext.SaveChangesAsync();

        return forecast;
    }

    private sealed record OpenWeatherForecastResponse(
        Main main,
        int dt,
        string name
    );

    private sealed record Main(
        double temp
    );
}

internal sealed class OpenWeatherMapOptions
{
    public const string Section = "OpenWeatherMap";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
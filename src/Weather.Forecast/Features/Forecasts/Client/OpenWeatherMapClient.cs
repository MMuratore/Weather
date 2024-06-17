using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Features.Forecasts.Client;

internal sealed class OpenWeatherMapClient(
    HttpClient client,
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
        var forecast = WeatherForecast.Create(date, new Temperature((decimal)response.main.temp), null);
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
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Features.Forecasts.Domain;

internal sealed class WeatherForecast : Entity<WeatherForecastId>, IAggregateRoot
{
    private WeatherForecast(DateOnly date, Temperature temperature, Summary? summary, MeteorologistId? meteorologistId)
    {
        Id = WeatherForecastId.Empty;
        Date = date;
        Temperature = temperature;
        Summary = summary;
        MeteorologistId = meteorologistId;
        AddDomainEvent(new WeatherForecastCreated(summary, meteorologistId));
    }
    
    internal static WeatherForecast Create(DateOnly date, Temperature temperature, Summary? summary, MeteorologistId? meteorologistId = null)
    {
        return new WeatherForecast(date, temperature, summary, meteorologistId == Guid.Empty ? null : meteorologistId);
    } 
    
    public DateOnly Date { get; private set; }
    public Temperature Temperature { get; private set; }
    public Summary? Summary { get; private set; }
    
    public MeteorologistId? MeteorologistId { get; private set; }
}

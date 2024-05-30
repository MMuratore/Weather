using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Domain;

internal sealed class WeatherForecast : Entity<WeatherForecastId>, IAggregateRoot
{
    private WeatherForecast(DateOnly date, Temperature temperature, Summary? summary)
    {
        Id = WeatherForecastId.Empty;
        Date = date;
        Temperature = temperature;
        Summary = summary;
        AddDomainEvent(new WeatherForecastCreated(summary));
    }
    
    internal static WeatherForecast Create(DateOnly date, Temperature temperature, Summary? summary)
    {
        return new WeatherForecast(date, temperature, summary);
    } 
    
    public DateOnly Date { get; private set; }
    public Temperature Temperature { get; private set; }
    public Summary? Summary { get; private set; }
}

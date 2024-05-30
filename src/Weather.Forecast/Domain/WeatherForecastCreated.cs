using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Domain;

internal sealed record WeatherForecastCreated(Summary? Summary) : IDomainEvent;

internal static class WeatherForecastCreatedMapper
{
    public static Contract.WeatherForecastCreated ToIntegrationEvent(
        this WeatherForecastCreated @event)
    {
        return new Contract.WeatherForecastCreated(@event.Summary?.ToString());
    }
}

using Weather.Forecast.Feature.Meteorologist.Domain.Event;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Feature.Meteorologist.Domain;

internal sealed class Meteorologist : Entity<MeteorologistId>, IAggregateRoot
{
    private int _forecastCount;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Meteorologist()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private Meteorologist(Name name, BirthDate birthDay)
    {
        Id = MeteorologistId.NewWeatherForecastId;
        Name = name;
        BirthDay = birthDay;
        _forecastCount = 0;
        AddDomainEvent(new MeteorologistCreated(Id, Name.Fullname));
    }

    public Name Name { get; private set; }
    public BirthDate BirthDay { get; private set; }

    public Prestige Prestige => _forecastCount switch
    {
        < 1 => Prestige.Noob,
        < 10 => Prestige.Casual,
        _ => Prestige.Expert
    };

    internal static Meteorologist Create(Name name, BirthDate birthDate) => new(name, birthDate);

    internal void IncrementForecast()
    {
        var oldPrestige = Prestige;
        _forecastCount++;
        if (oldPrestige != Prestige) AddDomainEvent(new MeteorologistPromoted(Id, Prestige));
    }
}
namespace Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;

internal sealed record Name(string Firstname, string Lastname)
{
    public string Fullname => $"{Firstname} {Lastname}";
}
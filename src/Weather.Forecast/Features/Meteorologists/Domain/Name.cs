namespace Weather.Forecast.Features.Meteorologists.Domain;

internal sealed record Name(string Firstname, string Lastname)
{
    public string Fullname => $"{Firstname} {Lastname}";
}
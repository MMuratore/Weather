using Asp.Versioning;

namespace Weather.SharedKernel;

public static class WeatherApiVersion
{
    public const string Name = "Weather.Api";
    public const string RequiredApiVersionHeaderName = "x-api-version";

    public static readonly ApiVersion DefaultApiVersion = new (1, 0);
}

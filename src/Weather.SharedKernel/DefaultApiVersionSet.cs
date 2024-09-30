using Asp.Versioning;

namespace Weather.SharedKernel;

public abstract class DefaultApiVersionSet
{
    public const string RequiredApiVersionHeaderName = "x-api-version";
    public static readonly ApiVersion DefaultApiVersion = new(1, 0);
}
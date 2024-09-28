using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Weather.Notification.Common.Cache;

internal sealed class InMemoryMeteorologistCache(IMemoryCache memoryCache, ILogger<InMemoryMeteorologistCache> logger)
    : IMeteorologistCache
{
    public Task<MeteorologistFullName?> GetByIdAsync(Guid? meteorologistId)
    {
        if (meteorologistId is null)
            return Task.FromResult<MeteorologistFullName?>(null);

        logger.LogInformation("retrieving meteorologist '{MeteorologistId}' from cache", meteorologistId);
        return Task.FromResult(memoryCache.Get<MeteorologistFullName>(meteorologistId));
    }

    public Task StoreAsync(MeteorologistFullName meteorologistFullName)
    {
        memoryCache.Set(meteorologistFullName.Id, meteorologistFullName);
        return Task.CompletedTask;
    }
}
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Weather.Notification;

internal sealed class InMemoryMeteorologistCache(IMemoryCache memoryCache, ILogger<InMemoryMeteorologistCache> logger)
    : IMeteorologistCache
{
    public Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId)
    {
        if (meteorologistId is null)
            return Task.FromResult<Meteorologist?>(null);

        logger.LogInformation("retrieving meteorologist '{MeteorologistId}' from cache", meteorologistId);
        return Task.FromResult(memoryCache.Get<Meteorologist>(meteorologistId));
    }

    public Task StoreAsync(Meteorologist meteorologist)
    {
        memoryCache.Set(meteorologist.Id, meteorologist);
        return Task.CompletedTask;
    }
}
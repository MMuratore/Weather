using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Weather.Notification;

internal sealed class InMemoryMeteorologistCache : IMeteorologistCache
{
    private readonly IMemoryCache _memoryCache;
    private ILogger<InMemoryMeteorologistCache> _logger;
    
    public InMemoryMeteorologistCache(IMemoryCache memoryCache, ILogger<InMemoryMeteorologistCache> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId)
    {
        if (meteorologistId is null)
            return Task.FromResult<Meteorologist?>(null);
        
        _logger.LogInformation("retrieving meteorologist '{MeteorologistId}' from cache", meteorologistId);
        return Task.FromResult(_memoryCache.Get<Meteorologist>(meteorologistId));
    }

    public Task StoreAsync(Meteorologist meteorologist)
    {
        _memoryCache.Set(meteorologist.Id, meteorologist);
        return Task.CompletedTask;
    }
}
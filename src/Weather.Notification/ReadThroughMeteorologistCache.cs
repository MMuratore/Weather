using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;

namespace Weather.Notification;

internal sealed class ReadThroughMeteorologistCache : IMeteorologistCache
{
    private readonly InMemoryMeteorologistCache _inMemoryCahce;
    private readonly IMediator _mediator;
    private readonly ILogger<ReadThroughMeteorologistCache> _logger;

    public ReadThroughMeteorologistCache(InMemoryMeteorologistCache inMemoryCahce, IMediator mediator, ILogger<ReadThroughMeteorologistCache> logger)
    {
        _inMemoryCahce = inMemoryCahce;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId)
    {
        if (meteorologistId is null) return null;
        
        var meteorologist = await _inMemoryCahce.GetByIdAsync(meteorologistId);
        if (meteorologist is not null) return meteorologist;
        
        _logger.LogInformation("meteorologist '{MeteorologistId}' not found in cache, fetching from source", meteorologistId);
        var query = new MeteorologistFullNameByIdQuery(meteorologistId.Value);
        var meteorologistFullname = await _mediator.Send(query);
        
        if(meteorologistFullname is null) return null;

        _logger.LogInformation("meteorologist '{MeteorologistId}' successfully fetch from source. hydrating it into cache", meteorologistId);
        await _inMemoryCahce.StoreAsync(new Meteorologist(meteorologistFullname.Id, meteorologistFullname.Fullname));
        return meteorologist;
    }

    public Task StoreAsync(Meteorologist meteorologist) => _inMemoryCahce.StoreAsync(meteorologist);
}
using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;

namespace Weather.Notification.Common.Cache;

internal sealed class ReadThroughMeteorologistCache(
    InMemoryMeteorologistCache inMemoryCahce,
    IMediator mediator,
    ILogger<ReadThroughMeteorologistCache> logger) : IMeteorologistCache
{
    public async Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId)
    {
        if (meteorologistId is null) return null;

        var meteorologist = await inMemoryCahce.GetByIdAsync(meteorologistId);
        if (meteorologist is not null) return meteorologist;

        logger.LogInformation("meteorologist '{MeteorologistId}' not found in cache, fetching from source",
            meteorologistId);
        var query = new GetMeteorologistFullNameById(meteorologistId.Value);
        var meteorologistFullname = await mediator.Send(query);

        if (meteorologistFullname is null) return null;

        logger.LogInformation(
            "meteorologist '{MeteorologistId}' successfully fetch from source. hydrating it into cache",
            meteorologistId);
        await inMemoryCahce.StoreAsync(new Meteorologist(meteorologistFullname.Id, meteorologistFullname.Fullname));
        return meteorologist;
    }

    public Task StoreAsync(Meteorologist meteorologist) => inMemoryCahce.StoreAsync(meteorologist);
}
using MediatR;
using Weather.Forecast.Contract;
using Weather.Notification.Common.Cache;

namespace Weather.Notification.Integration;

internal sealed class CacheNewMeteorologistHandler(IMeteorologistCache meteorologistCache)
    : INotificationHandler<MeteorologistCreated>
{
    public async Task Handle(MeteorologistCreated notification, CancellationToken cancellationToken)
    {
        var meteorologist = new Meteorologist(notification.MeteorologistId, notification.Fullname);

        await meteorologistCache.StoreAsync(meteorologist);
    }
}
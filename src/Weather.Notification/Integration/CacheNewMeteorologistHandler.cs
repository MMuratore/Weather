using MediatR;
using Weather.Forecast.Contract;

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
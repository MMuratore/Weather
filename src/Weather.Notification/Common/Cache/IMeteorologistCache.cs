namespace Weather.Notification.Common.Cache;

internal interface IMeteorologistCache
{
    Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId);

    Task StoreAsync(Meteorologist meteorologist);
}
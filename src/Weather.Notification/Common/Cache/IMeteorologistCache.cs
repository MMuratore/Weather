namespace Weather.Notification.Common.Cache;

internal interface IMeteorologistCache
{
    Task<MeteorologistFullName?> GetByIdAsync(Guid? meteorologistId);

    Task StoreAsync(MeteorologistFullName meteorologistFullName);
}
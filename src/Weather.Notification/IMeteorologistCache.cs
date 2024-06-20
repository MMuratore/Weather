namespace Weather.Notification;

internal interface IMeteorologistCache
{
    Task<Meteorologist?> GetByIdAsync(Guid? meteorologistId);

    Task StoreAsync(Meteorologist meteorologist);
}
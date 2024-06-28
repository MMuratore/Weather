using Weather.Forecast.Feature.Meteorologist.Domain.Error;
using Weather.SharedKernel.Exception;

namespace Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;

internal record BirthDate(DateOnly Date, TimeOnly? Hour = null)
{
    public DateOnly Date { get; init; } = Date <= DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().Date)
        ? Date
        : throw new ConflictException(BirthDateError.BirthDateCannotBeInFuture);

    public int Age => CalculateAge();

    private int CalculateAge()
    {
        var birthDateTime = Date.ToDateTime(Hour ?? new TimeOnly(0, 0));

        var now = TimeProvider.System.GetUtcNow();

        var age = now.Year - birthDateTime.Year;

        if (now.Month < birthDateTime.Month || (now.Month == birthDateTime.Month && now.Day < birthDateTime.Day)) age--;

        return age;
    }
}
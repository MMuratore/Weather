using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Features.Meteorologists.Domain;

internal record BirthDate(DateOnly Date, TimeOnly? Hour = null)
{
    public DateOnly Date { get; init; } = Date <= DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().Date) ? Date : throw new DomainException("birth date must be in the past");
    
    public int Age => CalculateAge();
    
    private int CalculateAge()
    {
        DateTime birthDateTime = Date.ToDateTime(Hour ?? new TimeOnly(0, 0));
        
        var now = TimeProvider.System.GetUtcNow();
        
        int age = now.Year - birthDateTime.Year;
        
        if (now.Month < birthDateTime.Month || (now.Month == birthDateTime.Month && now.Day < birthDateTime.Day))
        {
            age--;
        }
        
        return age;
    }
}

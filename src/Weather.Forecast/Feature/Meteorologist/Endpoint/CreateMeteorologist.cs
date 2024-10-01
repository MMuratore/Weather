using Bogus;
using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Meteorologist.Endpoint;

internal sealed class CreateMeteorologist(ForecastDbContext dbContext) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/meteorologists/random");
        Options(o => o.WithVersionSet(MeteorologistApiVersionSet.MeteorologistSet).MapToApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        Summary(s => { s.Summary = "generate a random meteorologist"; });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var (name, birthday) = CreateFakeData();

        var meteorologist = Domain.Meteorologist.Create(name, birthday);

        await dbContext.Set<Domain.Meteorologist>().AddAsync(meteorologist, ct);
        await dbContext.SaveChangesAsync(ct);

        await SendCreatedAtAsync<GetMeteorologist>(new { Id = (Guid)meteorologist.Id }, meteorologist.ToResponse(),
            cancellation: ct);
    }

    private static (Name name, BirthDate birthdate) CreateFakeData()
    {
        var faker = new Faker();
        var name = new Name(faker.Name.FirstName(), faker.Name.LastName());

        var date = faker.Date.PastDateOnly(70,
            DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().Date.AddYears(-20)));

        TimeOnly? hour = Random.Shared.Next(10) == 5
            ? faker.Date.BetweenTimeOnly(TimeOnly.MinValue, TimeOnly.MaxValue)
            : null;

        var birthdate = new BirthDate(date, hour);

        return (name, birthdate);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;

namespace Weather.Forecast.Feature.Forecast.Persistence;

internal sealed class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                v => (Guid)v,
                v => v)
            .ValueGeneratedNever();

        builder.Property(x => x.Temperature)
            .HasConversion(
                v => v.Celsius,
                v => new Temperature(v))
            .HasPrecision(5, 2)
            .HasColumnName(nameof(Temperature.Celsius));

        builder.Property(x => x.Summary).HasMaxLength(10);

        builder.HasOne<Meteorologist.Domain.Meteorologist>().WithMany().HasForeignKey(x => x.MeteorologistId);
    }
}
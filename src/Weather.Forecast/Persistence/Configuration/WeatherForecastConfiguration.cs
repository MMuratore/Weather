using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Meteorologists.Domain;

namespace Weather.Forecast.Persistence.Configuration;

internal sealed class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                v => (Guid)v,
                v => v)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Temperature)
            .HasConversion(
                v => v.Celsius,
                v => new Temperature(v))
            .HasPrecision(5, 2)
            .HasColumnName(nameof(Temperature.Celsius));
        
        builder.Property(x => x.Summary).HasMaxLength(10);
        
        builder.HasOne<Meteorologist>().WithMany().HasForeignKey(x => x.MeteorologistId);
    }
}

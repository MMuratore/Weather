using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Forecast.Features.Meteorologists.Domain;

namespace Weather.Forecast.Persistence.Configuration;

internal sealed class MeteorologistConfiguration : IEntityTypeConfiguration<Meteorologist>
{
    public void Configure(EntityTypeBuilder<Meteorologist> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                v => (Guid)v,
                v => v)
            .ValueGeneratedOnAdd();
        
        builder.ComplexProperty(x => x.Name, b =>
        {
            b.Property(x => x.Firstname).HasColumnName(nameof(Name.Firstname)).HasMaxLength(200);
            b.Property(x => x.Lastname).HasColumnName(nameof(Name.Lastname)).HasMaxLength(200);
        });
        builder.ComplexProperty(x => x.BirthDay);
        builder.Property("_forecastCount").HasColumnName("ForecastCount");
    }
}

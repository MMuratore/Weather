using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;

namespace Weather.Forecast.Feature.Meteorologist.Persistence;

internal sealed class MeteorologistConfiguration : IEntityTypeConfiguration<Domain.Meteorologist>
{
    public void Configure(EntityTypeBuilder<Domain.Meteorologist> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                v => (Guid)v,
                v => v)
            .ValueGeneratedNever();


        builder.ComplexProperty(x => x.Name, b =>
        {
            b.Property(x => x.Firstname).HasColumnName(nameof(Name.Firstname)).HasMaxLength(200);
            b.Property(x => x.Lastname).HasColumnName(nameof(Name.Lastname)).HasMaxLength(200);
        });
        builder.ComplexProperty(x => x.BirthDay);
        builder.Property("_forecastCount").HasColumnName("ForecastCount");
    }
}
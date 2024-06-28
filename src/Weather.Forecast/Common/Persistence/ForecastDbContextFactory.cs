using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weather.Forecast.Common.Persistence;

internal class ForecastDbContextFactory : IDesignTimeDbContextFactory<ForecastDbContext>
{
    public ForecastDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ForecastDbContext>();

        return new ForecastDbContext(optionsBuilder.Options);
    }
}
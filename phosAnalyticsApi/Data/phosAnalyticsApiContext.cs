using Microsoft.EntityFrameworkCore;

public class phosAnalyticsApiContext : DbContext
{
    public phosAnalyticsApiContext(DbContextOptions<phosAnalyticsApiContext> options)
        : base(options)
    {
    }

    public DbSet<phosAnalyticsApi.Models.ChartPoint> ChartPoint { get; set; }
    public DbSet<phosAnalyticsApi.Models.ChartData> ChartData { get; set; }
}

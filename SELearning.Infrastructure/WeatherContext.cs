namespace SELearning.Infrastructure;

public class WeatherContext : DbContext, IWeatherContext
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options) { }
}
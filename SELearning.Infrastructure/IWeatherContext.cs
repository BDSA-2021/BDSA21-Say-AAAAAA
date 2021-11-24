namespace SELearning.Infrastructure;

public interface IWeatherContext : IDisposable
{
    DbSet<WeatherForecast> WeatherForecasts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
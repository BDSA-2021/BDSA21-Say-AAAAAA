namespace SELearning.Infrastructure;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<WeatherForecastDTO[]> ReadAsync(DateTime startDate)
    {
        var rng = new Random();
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastDTO
        {
            Date = startDate.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }).ToArray());
    }
}
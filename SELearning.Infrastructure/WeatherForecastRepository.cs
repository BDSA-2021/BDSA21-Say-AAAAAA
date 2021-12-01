namespace SELearning.Infrastructure;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly IWeatherContext _context;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastRepository(IWeatherContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<WeatherForecastDTO>> ReadAsync(DateTime startDate)
    {
        return (await _context.WeatherForecasts
            .Select(w => new WeatherForecastDTO(
                w.Id,
                w.Date,
                w.TemperatureC,
                w.Summary
            ))
            .ToListAsync())
            .AsReadOnly();
    }

    public async Task<WeatherForecastDTO> CreateAsync(WeatherForecastCreateDTO weatherForecast)
    {
        var entity = new WeatherForecast
        {
            Date = weatherForecast.Date,
            TemperatureC = weatherForecast.TemperatureC,
            Summary = weatherForecast.Summary,
        };

        _context.WeatherForecasts.Add(entity);

        await _context.SaveChangesAsync();

        return new WeatherForecastDTO(
            entity.Id,
            entity.Date,
            entity.TemperatureC,
            entity.Summary
        );
    }

    public async Task<WeatherForecastDTO> Generate()
    {
        var rng = new Random();

        return await this.CreateAsync(new WeatherForecastCreateDTO
        {
            Date = DateTime.Now.AddDays(rng.Next(50)),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        });
    }
}
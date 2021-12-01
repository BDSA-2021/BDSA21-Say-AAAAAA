namespace SELearning.Core;

public interface IWeatherForecastRepository
{
    Task<WeatherForecastDTO> CreateAsync(WeatherForecastCreateDTO weatherForecast);
    Task<WeatherForecastDTO> Generate();
    Task<IReadOnlyCollection<WeatherForecastDTO>> ReadAsync(DateTime startDate);
}

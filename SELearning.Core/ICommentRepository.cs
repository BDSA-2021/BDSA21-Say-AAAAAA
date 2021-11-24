namespace SELearning.Core;

public interface ICommentRepository
{
    Task<WeatherForecastDTO> CreateAsync(WeatherForecastCreateDTO weatherForecast);
    Task<WeatherForecastDTO> Generate();
    Task<WeatherForecastDTO[]> ReadAsync(DateTime startDate);
}

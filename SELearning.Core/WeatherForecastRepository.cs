namespace SELearning.Core;

public interface IWeatherForecastRepository {
    Task<WeatherForecastDTO[]> ReadAsync(DateTime startDate);
}

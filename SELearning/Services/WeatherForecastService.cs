using System;
using System.Linq;
using System.Threading.Tasks;

namespace SELearning.Services;

public class WeatherForecastService
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastService(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public async Task<WeatherForecastDTO[]> Get()
    {
        return await _repository.ReadAsync(DateTime.Now);
    }

    public async Task<WeatherForecastDTO> Generate()
    {
        return await _repository.Generate();
    }
}

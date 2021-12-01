using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace SELearning.API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    // The Web API will only accept tokens 1) for users, and 2) having the "API.Access" scope for this API
    static readonly string[] scopeRequiredByApi = new string[] { "API.Access" };

    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<WeatherForecastDTO>> Get()
    {
        return await _repository.ReadAsync(DateTime.Now);
    }


    [HttpPost("generate")]
    public async Task<WeatherForecastDTO> Generate()
    {
        return await _repository.Generate();
    }
}

namespace SELearning.Core;


public record WeatherForecastCreateDTO
{
    public DateTime Date;
    public int TemperatureC;

    [StringLength(50)]
    public string? Summary;
}

public record WeatherForecastDTO : WeatherForecastCreateDTO
{
    public int Id;
}
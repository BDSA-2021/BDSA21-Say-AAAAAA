namespace SELearning.API.Models;

public static class Migrator
{
    public static IHost Migrate(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<WeatherContext>();

            ctx.Database.Migrate();
        }

        return host;
    }
}
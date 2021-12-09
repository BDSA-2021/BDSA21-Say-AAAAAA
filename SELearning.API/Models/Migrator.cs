namespace SELearning.API.Models;

public static class Migrator
{
    public static IHost Migrate(this IHost host)
    {
        Migrator.MigrateContext<SELearningContext>(host);

        return host;
    }

    private static void MigrateContext<T>(IHost host) where T : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<T>();

            ctx.Database.Migrate();
        }
    }
}
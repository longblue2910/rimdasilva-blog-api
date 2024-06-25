using Microsoft.EntityFrameworkCore;

namespace iTracks.Api.Extensions;

public static class HostExtensions
{
    public static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables();
    }

    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating sql database");
                ExcuteMigrations(context).Wait();
                logger.LogInformation("Migrating sql database");
                InvokeSeeder(seeder, context, services);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the mysql database");
            }
        }

        return host;
    }

    private async static Task ExcuteMigrations<TContext>(TContext context)
        where TContext : DbContext
    {
        await context.Database.MigrateAsync();
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        seeder(context, services);
    }
}

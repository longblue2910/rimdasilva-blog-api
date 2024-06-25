using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Post.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(PostDbContext).Assembly.FullName));
        });

        //services.AddTransient<ITokenService, TokenService>();
        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<ITrackRepository, TrackRepository>();

        services.AddScoped(typeof(IRepositoryBaseAsync<,>), typeof(RepositoryBase<,>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }
}

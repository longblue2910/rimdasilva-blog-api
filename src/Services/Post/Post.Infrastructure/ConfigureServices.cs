using Contracts.Common.Interfaces;
using Contracts.Interfaces;
using Infrastructure.Common;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.AggregatesModel.PostAggregate;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Infrastructure.Repositories;

namespace Post.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(PostDbContext).Assembly.FullName));
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<PostDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<UserManager<ApplicationUser>>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        services.AddScoped(typeof(IRepositoryBaseAsync<,>), typeof(RepositoryBase<,>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }
}

using Contracts.Common.Interfaces;
using Contracts.Interfaces;
using Infrastructure.Common;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.AggregatesModel.PostAggregate;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Infrastructure.Repositories;

namespace Post.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình MongoDB
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            // Cấu hình MongoClient
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Cấu hình MongoDbContext
            services.AddScoped<MongoDbContext>();

            // Đăng ký các repository
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();


            // Đăng ký các dịch vụ chung
            services.AddScoped(typeof(IRepositoryBaseAsync<,>), typeof(RepositoryBase<,>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            // Cấu hình quản lý người dùng
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}

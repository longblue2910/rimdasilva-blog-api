using Contracts.Configurations;
using Contracts.Identity;
using eShop.Ordering.API.Application.Behaviors;
using FluentValidation;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Post.Api.Application.Behaviors;
using Post.Api.Infrastructure;
using Post.Infrastructure;
using System.Reflection;
using System.Text;

namespace Post.Api.Extensions;
public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.Configure<RouteOptions>(options
                => options.LowercaseQueryStrings = true);

        services.AddMigration<PostDbContext, PostContextSeed>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure mediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
    }

    internal static void AddConfigurationSettings(this IServiceCollection services,
    IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings))
            .Get<JwtSettings>();
        services.AddSingleton(jwtSettings);

        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);
    }

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetValue<string>("AllowedOrigins").Split(";");
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        var setting = services.GetOptions<JwtSettings>(nameof(JwtSettings));
        if (setting == null || string.IsNullOrEmpty(setting.Key))
            throw new ArgumentNullException($"{nameof(JwtSettings)} is not configured propely.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.Key));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = false
        };
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = tokenValidationParameters;
        });
    }

}

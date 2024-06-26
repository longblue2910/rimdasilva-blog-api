﻿using Asp.Versioning;
using Contracts.Configurations;
using Contracts.Identity;
using eShop.Ordering.API.Application.Behaviors;
using FluentValidation;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Post.Api.Application.Behaviors;
using Post.Api.Infrastructure;
using Post.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;
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
    public static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder apiVersioning = default)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        services.AddEndpointsApiExplorer();

        if (apiVersioning is not null)
        {
            // the default format will just be ApiVersion.ToString(); for example, 1.0.
            // this will format the version as "'v'major[.minor][-status]"
            apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());
        }

        return builder;
    }

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        app.UseSwagger();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(setup =>
            {
                /// {
                ///   "OpenApi": {
                ///     "Endpoint: {
                ///         "Name": 
                ///     },
                ///     "Auth": {
                ///         "ClientId": ..,
                ///         "AppName": ..
                ///     }
                ///   }
                /// }

                var pathBase = configuration["PATH_BASE"] ?? string.Empty;
                var authSection = openApiSection.GetSection("Auth");
                var endpointSection = openApiSection.GetRequiredSection("Endpoint");

                foreach (var description in app.DescribeApiVersions())
                {
                    var name = description.GroupName;
                    var url = endpointSection["Url"] ?? $"{pathBase}/swagger/{name}/swagger.json";

                    setup.SwaggerEndpoint(url, name);
                }

                if (authSection.Exists())
                {
                    setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
                    setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
                }
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        return app;
    }

}

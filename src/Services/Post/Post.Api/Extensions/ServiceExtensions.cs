﻿using Asp.Versioning;
using Contracts.Identity;
using Core.Attributes;
using eShop.Ordering.API.Application.Behaviors;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Post.Api.Application.Behaviors;
using Post.Api.Applications.Queries.Post;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Post.Api.Extensions;
public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        services.AddControllers();
        services.AddSignalR();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
        services.AddOptions();

        services.Configure<RouteOptions>(options
                => options.LowercaseQueryStrings = true);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure mediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        services.AddScoped<IPostQueries, PostQueries>();


        //Common config
        services.AddControllers(config =>
        {
            config.Filters.Add(new ValidateModelAttribute());
        });

        services.AddHttpContextAccessor();
    }

    internal static void AddConfigurationSettings(this IServiceCollection services,
    IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings))
            .Get<JwtSettings>();
        services.AddSingleton(jwtSettings);
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

        if (/*app.Environment.IsDevelopment()*/ true)
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

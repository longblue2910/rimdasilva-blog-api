using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.FileProviders;
using Post.Api.Apis;
using Post.Api.Extensions;
using Post.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationSettings(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddJwtAuthentication();

builder.Services.ConfigureCors(builder.Configuration);

//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.ReportApiVersions = true;
//});


var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

//builder.AddDefaultOpenApi(builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>());

var app = builder.Build();

app.UseHttpsRedirection();

//Setting static file
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

var posts = app.NewVersionedApi("Posts");
posts.MapOrdersApiV1();

  posts.MapOrdersApiV2()
    .RequireAuthorization();



app.UseDefaultOpenApi();


app.Run();
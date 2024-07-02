using Microsoft.Extensions.FileProviders;
using Post.Api.Extensions;
using Post.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationSettings(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddAntiforgery();

builder.Services.AddOAuth(builder.Configuration);

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);



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

app.UseAntiforgery();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.ApiManage();

app.UseDefaultOpenApi();

app.Run();
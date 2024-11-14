using Microsoft.Extensions.FileProviders;
using Post.Api.Extensions;
using Post.Api.Infrastructure;
using Post.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationSettings(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddAntiforgery();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

builder.Services.AddTransient<PostContextSeed>();

var app = builder.Build();

// Seed dữ liệu khi ứng dụng khởi động
using (var scope = app.Services.CreateScope())
{
    var mongoContextSeed = scope.ServiceProvider.GetRequiredService<PostContextSeed>();
    await mongoContextSeed.SeedAsync();
}

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
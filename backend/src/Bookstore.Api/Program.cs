using System.Text.Json.Serialization;
using Bookstore.Api.Common;
using Bookstore.Api.Extensions;
using Bookstore.Api.Middlewares;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddAppProblemDetails();
builder.Services.AddAppOpenApi();

builder.Services.AddAppCors(builder.Configuration);

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddDataInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddAppServices();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
        throw;
    }
}

app.UseExceptionHandler();
app.UseStatusCodePages();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors(CorsExtension.FrontendPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar/v1");
}

app.UseAuthentication();
app.UseMiddleware<CurrentAccountMiddleware>();
app.UseAuthorization();
app.MapControllers();

var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = "/files",
    }
);

app.Run();

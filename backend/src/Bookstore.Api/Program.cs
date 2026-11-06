using System.Text.Json.Serialization;
using Bookstore.Api.Common;
using Bookstore.Api.Extensions;
using Bookstore.Api.Middlewares;
using Bookstore.Data;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddAppProblemDetails();
builder.Services.AddAppOpenApi();

builder.Services.AddAppCors(builder.Configuration);

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddDataInfrastructure(builder.Configuration);
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

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
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
        FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "uploads")
        ),
        RequestPath = "/files",
    }
);

app.Run();

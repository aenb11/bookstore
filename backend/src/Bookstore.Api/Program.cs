using Bookstore.Api.Extensions;
using Bookstore.Api.Middlewares;
using Bookstore.Api.Services;
using Bookstore.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddAppOpenApi();

builder.Services.AddAppCors(builder.Configuration);

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddDataInfrastructure(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrentAccountService, CurrentAccountService>();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.Run();

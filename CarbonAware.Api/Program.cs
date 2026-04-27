using CarbonAware.Api.Middleware;
using CarbonAware.Api.Services;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarbonAware.Api.Data;
using CarbonAware.Api.Hubs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// // This tells the app: "Whenever a class asks for ICarbonService, give it CarbonService"
// builder.Services.AddScoped<ICarbonService, CarbonService>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // No trailing slash!
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 1. Register the Middleware in the DI container
builder.Services.AddTransient<CarbonAwareMiddleware>();

// This registers BOTH the ICarbonService and a pre-configured HttpClient
builder.Services.AddHttpClient<ICarbonService, CarbonService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Connect to SQLite. "Data Source" is the name of the file it will create.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=carbon.db"));

builder.Services.AddHostedService<CarbonMonitoringWorker>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseCors();

app.UseMiddleware<CarbonAwareMiddleware>();

app.UseHttpsRedirection();

app.MapHub<CarbonHub>("/carbonHub");

app.UseAuthorization();

app.MapControllers();

app.Run();

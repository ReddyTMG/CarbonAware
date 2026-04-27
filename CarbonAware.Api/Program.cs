using CarbonAware.Api.Middleware;
using CarbonAware.Api.Services;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarbonAware.Api.Data;
using CarbonAware.Api.Hubs;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 10;           // Max 10 requests
        options.Window = TimeSpan.FromSeconds(10); // Per 10 seconds
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;             // Only queue 2 extra requests before rejecting
    });

    // Custom response when a user is blocked
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Slow down!", token);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseRateLimiter();

app.UseCors();

app.UseMiddleware<CarbonAwareMiddleware>();

app.UseHttpsRedirection();

app.MapHub<CarbonHub>("/carbonHub");

app.UseAuthorization();

app.MapControllers();

app.Run();

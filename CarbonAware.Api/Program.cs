using CarbonAware.Api.Middleware;
using CarbonAware.Api.Services;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// // This tells the app: "Whenever a class asks for ICarbonService, give it CarbonService"
// builder.Services.AddScoped<ICarbonService, CarbonService>();

// 1. Register the Middleware in the DI container
builder.Services.AddTransient<CarbonAwareMiddleware>();

// This registers BOTH the ICarbonService and a pre-configured HttpClient
builder.Services.AddHttpClient<ICarbonService, CarbonService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<CarbonAwareMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

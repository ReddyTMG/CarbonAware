using CarbonAware.Api.Data;
using CarbonAware.Api.Models;
using Microsoft.AspNetCore.SignalR;
using CarbonAware.Api.Hubs;

namespace CarbonAware.Api.Services;

public class CarbonMonitoringWorker : BackgroundService
{
    private readonly ILogger<CarbonMonitoringWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);
    private readonly IHubContext<CarbonHub> _hubContext;

    public CarbonMonitoringWorker(ILogger<CarbonMonitoringWorker> logger, IServiceScopeFactory scopeFactory, IHubContext<CarbonHub> hubContext)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Carbon Monitoring Worker is starting.");

        using PeriodicTimer timer = new(_checkInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                _logger.LogInformation("Worker fetching carbon data at: {time}", DateTimeOffset.Now);

                // Create a manual Scope to talk to the Database
                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    var carbonService = scope.ServiceProvider.GetRequiredService<ICarbonService>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var intensity = await carbonService.GetCurrentIntensityAsync();

                    var log = new CarbonLog
                    {
                        Timestamp = DateTime.UtcNow,
                        Intensity = intensity,
                        Rating = intensity < 150 ? "Green" : (intensity < 250 ? "Moderate" : "High")
                    };

                    dbContext.CarbonLogs.Add(log);
                    await dbContext.SaveChangesAsync(stoppingToken);

                    await _hubContext.Clients.All.SendAsync("ReceiveNewLog", log, stoppingToken);

                    _logger.LogInformation("Broadcasted new log to SignalR clients.");

                    _logger.LogInformation("Successfully saved carbon log: {Intensity} gCO2/kWh", intensity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching/saving carbon data.");
            }
        }
    }
}
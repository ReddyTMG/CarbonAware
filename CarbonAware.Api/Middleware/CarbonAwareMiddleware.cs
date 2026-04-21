using CarbonAware.Api.Services;

namespace CarbonAware.Api.Middleware;

public class CarbonAwareMiddleware : IMiddleware
{
    private readonly ICarbonService _carbonService;
    private readonly ILogger<CarbonAwareMiddleware> _logger;

    public CarbonAwareMiddleware(ICarbonService carbonService, ILogger<CarbonAwareMiddleware> logger)
    {
        _carbonService = carbonService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 1. Get the current intensity
        var intensity = await _carbonService.GetCurrentIntensityAsync();

        // 2. Log it to the terminal
        _logger.LogInformation("Request processed. Current Grid Intensity: {Intensity} gCO2/kWh", intensity);

        // 3. Add a custom header to the response
        context.Response.Headers.Append("X-Carbon-Intensity", intensity.ToString());

        // 4. Let the request continue to the Controller
        await next(context);
    }
}
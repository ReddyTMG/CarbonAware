using Microsoft.AspNetCore.Mvc; // This fixes ControllerBase, ApiController, and HttpGet
using CarbonAware.Api.Services; // This fixes ICarbonService (assuming your service is in this folder)
using CarbonAware.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CarbonAware.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CarbonController : ControllerBase
{
    private readonly ICarbonService _carbonService;
    private readonly ApplicationDbContext _context;

    // Inject both the service and the database context
    public CarbonController(ICarbonService carbonService, ApplicationDbContext context)
    {
        _carbonService = carbonService;
        _context = context;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        // throw new Exception("Simulated API Crash!"); // Temporarily add this to test the ExceptionHandlingMiddleware
        var intensity = await _carbonService.GetCurrentIntensityAsync();
        return Ok(new { 
            Intensity = intensity, 
            Message = intensity < 150 ? "Green: Great time to run jobs!" : "Red: Avoid heavy tasks." 
        });
    }

    // NEW: Get the history of logs
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        // Use LINQ to get the 20 most recent logs
        var logs = await _context.CarbonLogs
            .OrderByDescending(l => l.Timestamp)
            .Take(20)
            .ToListAsync();

        return Ok(logs);
    }
}
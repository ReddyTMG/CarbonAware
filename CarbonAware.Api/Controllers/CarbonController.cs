using Microsoft.AspNetCore.Mvc; // This fixes ControllerBase, ApiController, and HttpGet
using CarbonAware.Api.Services; // This fixes ICarbonService (assuming your service is in this folder)

namespace CarbonAware.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CarbonController : ControllerBase
{
    private readonly ICarbonService _carbonService;

    public CarbonController(ICarbonService carbonService)
    {
        _carbonService = carbonService;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var intensity = await _carbonService.GetCurrentIntensityAsync();
        return Ok(new { 
            Intensity = intensity, 
            Message = intensity < 150 ? "Green: Great time to run jobs!" : "Red: Avoid heavy tasks." 
        });
    }
}
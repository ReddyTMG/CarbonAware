namespace CarbonAware.Api.Services;

public interface ICarbonService
{
    Task<int> GetCurrentIntensityAsync();
}
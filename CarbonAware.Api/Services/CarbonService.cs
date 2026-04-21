using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CarbonAware.Api.Services;

public class CarbonService : ICarbonService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public CarbonService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<int> GetCurrentIntensityAsync()
    {
        var baseUrl = _config["CarbonApi:BaseUrl"];
        // The UK API returns data at /intensity
        var response = await _httpClient.GetFromJsonAsync<CarbonResponse>($"{baseUrl}intensity");

        return response?.Data?.FirstOrDefault()?.Intensity?.Actual ?? 0;
    }
}

// These "DTO" classes help C# understand the JSON structure coming from the API
public record CarbonResponse(
    [property: JsonPropertyName("data")] List<IntensityData> Data
);

public record IntensityData(
    [property: JsonPropertyName("intensity")] IntensityDetails Intensity
);

public record IntensityDetails(
    [property: JsonPropertyName("actual")] int Actual
);
namespace CarbonAware.Api.Models;

public class CarbonLog
{
    public int Id { get; set; } // EF Core automatically makes this the Primary Key
    public DateTime Timestamp { get; set; }
    public int Intensity { get; set; }
    public string Rating { get; set; } = string.Empty; // e.g., "Green", "Moderate", "High"
}
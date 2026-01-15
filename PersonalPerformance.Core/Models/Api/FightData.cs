using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class FightData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("kill")]
    public bool? Kill { get; set; }
    
    [JsonPropertyName("startTime")]
    public double StartTime { get; set; }
    
    [JsonPropertyName("endTime")]
    public double EndTime { get; set; }
}
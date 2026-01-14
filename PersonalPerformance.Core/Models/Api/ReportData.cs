using System.Text.Json.Serialization;

public class ReportData
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonPropertyName("startTime")]
    public long StartTime { get; set; }
    
    [JsonPropertyName("fights")]
    public List<FightData>? Fights { get; set; }
}
using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class CharacterResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("server")]
    public ServerResponse Server { get; set; } = new();
    
    [JsonPropertyName("recentReports")]
    public RecentReportsResponse? RecentReports { get; set; }
}
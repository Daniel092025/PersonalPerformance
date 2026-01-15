using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class ServerResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
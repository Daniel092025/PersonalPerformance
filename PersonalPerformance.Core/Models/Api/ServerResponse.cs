using System.Text.Json.Serialization;

public class ServerResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
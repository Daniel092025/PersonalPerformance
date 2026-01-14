using System.Text.Json.Serialization;

public class RecentReportsResponse
{
    [JsonPropertyName("data")]
    public List<ReportData> Data { get; set; } = new();
}
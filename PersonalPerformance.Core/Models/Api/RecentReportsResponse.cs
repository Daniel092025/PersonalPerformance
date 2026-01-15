using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class RecentReportsResponse
{
    [JsonPropertyName("data")]
    public List<ReportData> Data { get; set; } = new();
}
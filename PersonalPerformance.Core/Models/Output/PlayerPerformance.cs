namespace PersonalPerformance.Core.Models;

public class PlayerPerformance
{
    public string CharacterName { get; set; } = string.Empty;
    public string Servername { get; set; } = string.Empty;
    public List<ReportSummary> RecentReports { get; set; } = new();

    // Her kan jeg legge til flere felt for henting i API
    // For eksempel: rankings, parses, item level, etc.
}




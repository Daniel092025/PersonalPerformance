namespace PersonalPerformance.Core.Models;

public class PlayerPerformance
{
    public string CharacterName { get; set; } = string.Empty;
    public string Servername { get; set; } = string.Empty;
    public List<ReportSummary> RecentReports { get; set; } = new();

    // Her kan jeg legge til flere felt for henting i API
    // For eksempel: rankings, parses, item level, etc.
}

public class ReportSummary
{
    public string ReportCode { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<FightPerformance> Fights { get; set; } = new();
}

public class FightPerformance
{
    public int FightId { get; set; }
    public string BossName { get; set; } = string.Empty;
    public double Dps { get; set; }
    public int Deaths { get; set; }
    public double DurationMs { get; set; }
    public bool Kill { get; set; }
}
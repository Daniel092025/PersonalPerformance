namespace PersonalPerformance.Core.Models;

public class ReportSummary
{
    public string ReportCode { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<FightPerformance> Fights { get; set; } = new();
}
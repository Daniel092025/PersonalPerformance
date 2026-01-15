namespace PersonalPerformance.Core.Models;

public class FightPerformance
{
    public int FightId { get; set; }
    public string BossName { get; set; } = string.Empty;
    public double Dps { get; set; }
    public int Deaths { get; set; }
    public double DurationMs { get; set; }
    public bool? Kill { get; set; }
}
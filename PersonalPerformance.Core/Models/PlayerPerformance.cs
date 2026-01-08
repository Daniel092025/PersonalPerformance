namespace PersonalPerformance.Core.Models;

public class PlayerPerformance
{
    public string CharacterName { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;

    // Add more properties as needed based on Warcraft Logs API response
    // For example: rankings, parses, item level, etc.
}

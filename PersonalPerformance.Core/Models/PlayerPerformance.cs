namespace PersonalPerformance.Core.Models;

public class PlayerPerformance
{
    public string CharacterName { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;

    // Her kan jeg legge til flere felt for henting i API
    // For eksempel: rankings, parses, item level, etc.
}

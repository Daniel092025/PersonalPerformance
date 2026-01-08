using PersonalPerformance.Core.Models;

namespace PersonalPerformance.Core.Services;

public interface IWarcraftLogsService
{
    Task<string> GetAccessTokenAsync();
    Task<PlayerPerformance> GetPlayerPerformanceAsync(string characterName, string server, string region);
}

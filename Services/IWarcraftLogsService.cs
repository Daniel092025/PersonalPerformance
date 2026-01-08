public interface IWarcraftLogsService
{
    Task<string> GetAccessTokenAsync();
    Task<PlayerPerformance> GetPlayerPerformanceAsync(string characterName, string server, string region);
}
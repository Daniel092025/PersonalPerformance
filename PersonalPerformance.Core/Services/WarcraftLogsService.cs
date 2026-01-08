using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PersonalPerformance.Core.Models;

namespace PersonalPerformance.Core.Services;

public class WarcraftLogsService : IWarcraftLogsService
{
    private readonly HttpClient _httpClient;
    private readonly WarcraftLogsConfig _config;
    private string? _cachedToken; // We'll improve this later

    public WarcraftLogsService(HttpClient httpClient, WarcraftLogsConfig config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        // OAuth2 Client Credentials flow
        var authValue = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}")
        );

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", authValue);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await _httpClient.PostAsync(
            "https://www.warcraftlogs.com/oauth/token",
            content
        );

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

        return tokenResponse?.AccessToken ?? throw new InvalidOperationException("No token received");
    }

    public async Task<PlayerPerformance> GetPlayerPerformanceAsync(string characterName, string server, string region)
    {
        throw new NotImplementedException();
    }
}

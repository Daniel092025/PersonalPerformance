using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;
using PersonalPerformance.Core.Models;

namespace PersonalPerformance.Core.Services;

public class WarcraftLogsService : IWarcraftLogsService
{
    private readonly HttpClient _httpClient;
    private readonly WarcraftLogsConfig _config;
    private string? _cachedToken; 
    private GraphQLHttpClient? _graphQLClient;

    public WarcraftLogsService(HttpClient httpClient, WarcraftLogsConfig config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    private async Task<GraphQLHttpClient> GetGraphQLHttpClientAsync()
    {
        if (_graphQLClient == null)
        {
            var token = await GetAccessTokenAsync();
            _graphQLClient = new GraphQLHttpClient(
                _config.ApiBaseUrl + "/client",
                new SystemTextJsonSerializer()
            );

            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue ("Bearer", token);
        }

        return _graphQLClient;
    }

    public async Task<PlayerPerformance> GetPlayerPerformanceAsync(
        string characterName,
        string server,
        string region)
    {
        var client = new GetGraphQLClientAsync();

        // GraphQL query - Vi spesifiserer akkurat det vi vil ha
        var query = new GraphQLRequest
        {
            Query = @"
                query($name: String!, $server: String!, $region: String!) {
                    characterData {
                        character(name: $name, serverSlug: $server, serverRegion: $region) {
                            name
                            server {
                                name
                            }
                            recentReports(limit: 5){
                                data {
                                    code
                                    title
                                    startTime
                                    fights(translate: true) {
                                        id
                                        name
                                        kill
                                        averageDps
                                        startTime
                                        endTime
                                    }
                                }
                            }
                        }
                    }
                }",
            Variables = new
            {
                name = characterName,
                server = server.ToLower(),
                region = region.ToUpper()
            }
        };

        try
        {
            var response = await client.SendQueryAsync<GraphQLResponse>(query);

            if(response.Errors?.Length > 0)
            {
                throw new InvalidOperationException(
                    $"GraphQL errors: {string.Join(",", response.Errors.Select(e => e.Message))}"
                
                );
            }

            return MapToPlayerPerformance(response.Data, characterName, server);

        }

        catch (Exception ex)
        {
            
        }

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

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PersonalPerformance.Core.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace PersonalPerformance.Core.Services;

public class WarcraftLogsService : IWarcraftLogsService
{
    private readonly HttpClient _httpClient;
    private readonly WarcraftLogsConfig _config;
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
        var client = await GetGraphQLHttpClientAsync();

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
            Console.WriteLine($"Error fetching player data: {ex.Message}");
            throw new InvalidOperationException(
                $"Failed to fetch data for {characterName}-{server}", ex
            );
        }
    }

    private PlayerPerformance MapToPlayerPerformance(
        GraphQLResponse data,
        string characterName,
        string server)
    {
        var character = data.CharacterData?.Character;

        if (character == null)
        {
            return new PlayerPerformance
            {
                CharacterName = characterName,
                Servername = server,
                RecentReports = new List<ReportSummary>()
            };
        }
        var performance = new PlayerPerformance
        {
            CharacterName = character.Name,
            Servername = character.Server.Name,
            RecentReports = new List<ReportSummary>()
        };

        foreach (var report in character.RecentReports?.Data ?? Enumerable.Empty<ReportData>())
        {
            var summary = new ReportSummary
            {
                ReportCode = report.Code,
                Title = report.Title,
                StartTime = DateTimeOffset.FromUnixTimeMilliseconds(report.StartTime).DateTime,
                Fights = new List<FightPerformance>()
            };

            foreach (var fights in report.Fights ?? Enumerable.Empty<FightData>())
            {
                summary.Fights.Add(new FightPerformance
                {
                    FightId = fights.Id,
                    BossName = fights.Name,
                    Dps = fights.AverageDPS ?? 0,
                    Kill = fights.Kill,
                    DurationMs = fights.EndTime - fights.StartTime
                });
            }

            performance.RecentReports.Add(summary);
        }
        return performance;

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
}

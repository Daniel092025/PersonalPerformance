using Microsoft.Extensions.Configuration;
using PersonalPerformance.Core.Models;
using PersonalPerformance.Core.Services;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var config = new WarcraftLogsConfig();
configuration.GetSection("WarcraftLogs").Bind(config);

using var httpClient = new HttpClient();
var service = new WarcraftLogsService(httpClient, config);

try
{
    var token = await service.GetAccessTokenAsync();
    Console.WriteLine($"Successfully retrieved token: {token[..20]}...");
}
catch (Exception ex)
{
    Console.WriteLine($"Error getting token: {ex.Message}");
}
using Microsoft.Extensions.Configuration;
using PersonalPerformance.Core.Models;
using PersonalPerformance.Core.Services;
using PersonalPerformance.Core.Configuration;


var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var warcraftConfig = config.GetSection("WarcraftLogs").Get<WarcraftLogsConfig>()
    ?? throw new InvalidOperationException("Configuration missing");

var httpClient = new HttpClient();
var service = new WarcraftLogsService(httpClient, warcraftConfig);

Console.WriteLine("Enter character name: ");
var characterName = Console.ReadLine() ?? "";

Console.WriteLine("Enter server name (e.g., 'tarren-mill'):");
var server = Console.ReadLine() ?? "";

Console.Write("Enter region (e.g., 'EU' or 'US'): ");
var region = Console.ReadLine() ?? "";

Console.WriteLine ("\nFetching performance data... \n");

var performance = await service.GetPlayerPerformanceAsync(characterName, server, region);
Console.WriteLine($"Character: {performance.CharacterName} - {performance.Servername}");
Console.WriteLine($"Recent Reports: {performance.RecentReports.Count}\n");

foreach (var report in performance.RecentReports)
{
    Console.WriteLine($"{report.Title}");
    Console.WriteLine($"    Date: {report.StartTime:yyyy-MM-dd HH:mm}");
    Console.WriteLine($"    Link: https://www.warcraftlogs.com/reports/{report.ReportCode}\n");

        foreach (var fight in report.Fights.Take(5)) // Show first 5 fights
    {
        var status = fight.Kill == true ? "✓ KILL" : "✗ WIPE";
        Console.WriteLine($"   {status} | {fight.BossName}");
        Console.WriteLine($"      DPS: {fight.Dps:N0} | Duration: {fight.DurationMs / 1000:N0}s");
    }
    Console.WriteLine();
}

/* Test for å få en token
try
{
    var token = await service.GetAccessTokenAsync();
    Console.WriteLine($"Successfully retrieved token: {token[..20]}...");
}
catch (Exception ex)
{
    Console.WriteLine($"Error getting token: {ex.Message}");
}
 */
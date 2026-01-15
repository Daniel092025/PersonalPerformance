using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class CharacterDataResponse
{
    [JsonPropertyName("character")]
    public CharacterResponse? Character { get; set; }
}
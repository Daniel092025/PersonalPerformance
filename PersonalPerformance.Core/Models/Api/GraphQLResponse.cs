using System.Text.Json.Serialization;

namespace PersonalPerformance.Core.Models.Api;

public class GraphQLResponse
{
    [JsonPropertyName("characterData")]
    public CharacterDataResponse? CharacterData { get; set; }
}
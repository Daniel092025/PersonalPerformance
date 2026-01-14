using System.Text.Json.Serialization;

public class GraphQLResponse
{
    [JsonPropertyName("characterData")]
    public CharacterDataResponse? CharacterData { get; set; }
}
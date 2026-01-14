using System.Text.Json.Serialization;

public class CharacterDataResponse
{
    [JsonPropertyName("character")]
    public CharacterResponse? Character { get; set; }
}
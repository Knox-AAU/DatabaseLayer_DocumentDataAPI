using System.Text.Json.Serialization;

namespace DocumentDataAPI.Models.Lemmatizer;

public class LemmatizerRequestModel
{
    public LemmatizerRequestModel(string input, string language)
    {
        String = input;
        Language = language;
    }

    [JsonPropertyName("string")]
    public string String { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }
}

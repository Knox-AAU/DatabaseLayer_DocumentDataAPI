using System.Text.Json.Serialization;

namespace DocumentDataAPI.Models.Lemmatizer;

public class LemmatizerResponseModel
{
    [JsonPropertyName("lemmatized_string")]
    public string LemmatizedString { get; set; } = null!;
}

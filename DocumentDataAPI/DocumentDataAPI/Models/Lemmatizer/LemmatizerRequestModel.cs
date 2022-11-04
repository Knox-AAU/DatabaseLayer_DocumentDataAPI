namespace DocumentDataAPI.Models.Lemmatizer;

public class LemmatizerRequestModel
{
    public LemmatizerRequestModel(string input, string language)
    {
        Input = input;
        Language = language;
    }

    public string Input { get; }
    public string Language { get; }
}

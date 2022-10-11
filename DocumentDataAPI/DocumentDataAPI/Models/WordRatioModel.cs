namespace DocumentDataAPI.Models;

public class WordRatioModel
{
    public WordRatioModel()
    {
    }

    public WordRatioModel(int documentId, string? word)
    {
        DocumentId = documentId;
        Word = word;
    }

    public WordRatioModel(int amount, int documentId, double percent, Ranks rank, string? word)
    {
        Amount = amount;
        DocumentId = documentId;
        Percent = percent;
        Rank = rank;
        Word = word;
    }

    public int Amount { get; init; }
    public int DocumentId { get; init; }
    public double Percent { get; init; }
    public Ranks Rank { get; init; }
    public string? Word { get; init; }
}

public enum Ranks
{
    Body,
    Synopsis,
    Subtitle,
    Title
}

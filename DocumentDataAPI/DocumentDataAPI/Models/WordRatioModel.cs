namespace DocumentDataAPI.Models;

public class WordRatioModel
{
    public WordRatioModel()
    {
    }

    public WordRatioModel(long documentId, string word)
    {
        DocumentId = documentId;
        Word = word;
    }

    public WordRatioModel(int amount, long documentId, double percent, Rank rank, string word)
    {
        Amount = amount;
        DocumentId = documentId;
        Percent = percent;
        Rank = rank;
        Word = word;
    }

    public int Amount { get; init; }
    public long DocumentId { get; init; }
    public double Percent { get; init; }
    public Rank Rank { get; init; }
    public string Word { get; init; } = null!;
}

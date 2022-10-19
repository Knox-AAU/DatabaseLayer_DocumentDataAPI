using System.ComponentModel.DataAnnotations;

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

    [Required]
    public long DocumentId { get; init; }
    [Required]
    public string Word { get; init; } = null!;
    [Required]
    public int Amount { get; init; }
    [Required]
    public double Percent { get; init; }
    [Required]
    public Rank Rank { get; init; }
}

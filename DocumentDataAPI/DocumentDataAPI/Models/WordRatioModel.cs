using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

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

    public WordRatioModel(int amount, long documentId, double percent, Rank rank, string word, float clusteringScore) :
        this(amount, documentId, percent, rank, word)
    {
        ClusteringScore = clusteringScore;
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
    [Required]
    public float ClusteringScore { get; init; }
    [ExcludeFromGeneratedInsertStatement]
    public float TfIdf { get; set; }
}

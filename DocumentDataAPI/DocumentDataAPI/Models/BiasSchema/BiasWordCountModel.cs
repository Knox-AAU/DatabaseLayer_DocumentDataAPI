using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models.BiasSchema;

public class BiasWordCountModel
{
    public BiasWordCountModel()
    {
    }

    public BiasWordCountModel(long id, string word, long count, float wordFrequency)
    {
        Id = id;
        Word = word;
        Count = count;
        WordFrequency = wordFrequency;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public long Id { get; init; }
    [Required]
    public string Word { get; init; } = null!;
    [Required]
    public long Count { get; init; }
    [Required]
    public float WordFrequency { get; init; }
}
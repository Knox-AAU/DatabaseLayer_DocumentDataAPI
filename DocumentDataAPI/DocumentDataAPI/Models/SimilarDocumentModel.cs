using Microsoft.Build.Framework;

namespace DocumentDataAPI.Models;

public class SimilarDocumentModel
{
    public SimilarDocumentModel()
    {

    }

    public SimilarDocumentModel(long mainDocumentId, long similarDocumentId, float similarity)
    {
        MainDocumentId = mainDocumentId;
        SimilarDocumentId = similarDocumentId;
        Similarity = similarity;
    }

    [Required]
    public long MainDocumentId { get; init; }
    [Required]
    public long SimilarDocumentId { get; init; }
    [Required]
    public float Similarity { get; init; }
}

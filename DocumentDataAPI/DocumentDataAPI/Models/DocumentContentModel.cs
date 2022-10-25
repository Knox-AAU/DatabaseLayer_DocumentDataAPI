using System.ComponentModel.DataAnnotations;

namespace DocumentDataAPI.Models;

public class DocumentContentModel
{
    public DocumentContentModel()
    {
    }

    public DocumentContentModel(long documentId, int index)
    {
        DocumentId = documentId;
        Index = index;
    }

    public DocumentContentModel(long documentId, int index, string content, string subheading) : this(documentId, index)
    {
        Content = content;
        Subheading = subheading;
    }

    [Required]
    public long DocumentId { get; init; }
    [Required]
    public int Index { get; init; }
    [Required]
    public string? Subheading { get; init; }
    [Required]
    public string Content { get; init; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace DocumentDataAPI.Models;

public class DocumentContentModel
{
    public DocumentContentModel()
    {
    }

    public DocumentContentModel(string content, long documentId)
    {
        Content = content;
        DocumentId = documentId;
    }

    [Required]
    public long DocumentId { get; init; }
    [Required]
    public string Content { get; init; } = null!;
}

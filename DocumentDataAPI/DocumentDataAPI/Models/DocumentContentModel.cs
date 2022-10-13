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

    public string Content { get; init; } = null!;
    public long DocumentId { get; init; }
}

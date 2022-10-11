namespace DocumentDataAPI.Models;

public class DocumentContentModel
{
    public DocumentContentModel()
    {
    }

    public DocumentContentModel(string content, int documentId)
    {
        Content = content;
        DocumentId = documentId;
    }

    public string Content { get; init; } = string.Empty;
    public int DocumentId { get; init; }
}

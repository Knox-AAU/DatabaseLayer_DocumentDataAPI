namespace DocumentDataAPI.Models;

public class DocumentContentModel
{
    public DocumentContentModel()
    {
    }

    public DocumentContentModel(string? content, int documentId)
    {
        Content = content;
        DocumentId = documentId;
    }

    public string? Content { get; init; }
    public int DocumentId { get; init; }
}
namespace DocumentDataAPI.Models;

public class DocumentModel
{
    public DocumentModel()
    {
    }

    public DocumentModel(string author, DateTime date, long id, string path, int sourceId, string summary, string title,
        int totalWords)
    {
        Author = author;
        Date = date;
        Id = id;
        Path = path;
        SourceId = sourceId;
        Summary = summary;
        Title = title;
        TotalWords = totalWords;
    }

    public string Author { get; init; } = null!;
    public DateTime Date { get; init; }
    public long Id { get; init; }
    public string Path { get; init; } = null!;
    public int SourceId { get; init; }
    public string Summary { get; init; } = null!;
    public string Title { get; init; } = null!;
    public int TotalWords { get; init; }
}
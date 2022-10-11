namespace DocumentDataAPI.Models;

public class DocumentModel
{
    public DocumentModel()
    {
    }

    public DocumentModel(string? author, DateTime date, int id, string? path, int sourceId, string? summary,
        string? title, int totalWords)
    {
        Author = author;
        Date = date;
        Id = id;
        Path = path;
        Source_Id = sourceId;
        Summary = summary;
        Title = title;
        TotalWords = totalWords;
    }

    public string? Author { get; init; }
    public DateTime Date { get; init; }
    public int Id { get; init; }
    public string? Path { get; init; }
    public int Source_Id { get; init; }
    public string? Summary { get; init; }
    public string? Title { get; init; }
    public int TotalWords { get; init; }
}

using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models;

public class DocumentModel
{
    public DocumentModel()
    {
    }

    public DocumentModel(string author, DateTime date, long id, string path, int sourceId, string? summary, string title, int totalWords, int categoryId, string? publication, int uniqueWords)
    {
        Author = author;
        Date = date;
        Id = id;
        Path = path;
        SourceId = sourceId;
        Summary = summary;
        Title = title;
        TotalWords = totalWords;
        CategoryId = categoryId;
        Publication = publication;
        UniqueWords = uniqueWords;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public long Id { get; init; }
    [Required]
    public int SourceId { get; init; }
    [Required]
    public int CategoryId { get; init; }
    public string? Publication { get; init; }
    [Required]
    public string Title { get; init; } = null!;
    [Required]
    public string Path { get; init; } = null!;
    public string? Summary { get; init; }
    [Required]
    public DateTime Date { get; init; }
    [Required]
    public string Author { get; init; } = null!;
    [Required]
    public int TotalWords { get; init; }
    [Required]
    public int UniqueWords { get; init; }
}

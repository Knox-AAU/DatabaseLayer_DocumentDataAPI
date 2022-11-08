using DocumentDataAPI.Data.Mappers;

namespace DocumentDataAPI.Models;

public class DocumentSearchParameters : ISearchParameters
{
    public List<QueryParameter> Parameters { get; }
    public DocumentSearchParameters()
    {
        Parameters = new();
    }

    public DocumentSearchParameters(long? sourceId, string? author, int? categoryId, DateTime? beforeDate, DateTime? afterDate)
        : this()
    {
        if (sourceId is not null) AddSource(sourceId.Value);
        if (author is not null) AddAuthor(author);
        if (categoryId is not null) AddCategory(categoryId.Value);
        if (beforeDate is not null) AddBeforeDate(beforeDate.Value);
        if (afterDate is not null) AddAfterDate(afterDate.Value);
    }

    public DocumentSearchParameters AddSource(long sourceId)
    {
        Parameters.Add(new QueryParameter(DocumentMap.SourceId, sourceId));
        return this;
    }

    public DocumentSearchParameters AddAuthor(string authorName)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Author, authorName));
        return this;
    }

    public DocumentSearchParameters AddCategory(int categoryId)
    {
        Parameters.Add(new QueryParameter(DocumentMap.CategoryId, categoryId));
        return this;
    }

    public DocumentSearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Date, date, "<="));
        return this;
    }

    public DocumentSearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Date, date, ">="));
        return this;
    }
}

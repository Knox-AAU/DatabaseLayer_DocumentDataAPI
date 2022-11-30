using DocumentDataAPI.Data.Mappers;

namespace DocumentDataAPI.Models;

public class DocumentSearchParameters : ISearchParameters
{
    public List<QueryParameter> Parameters { get; }
    public DocumentSearchParameters()
    {
        Parameters = new List<QueryParameter>();
    }

    public DocumentSearchParameters(List<long> sourceIds, List<string> authors, List<int> categoryIds, DateTime? beforeDate, DateTime? afterDate)
        : this()
    {
        if (sourceIds.Any()) AddSources(sourceIds);
        if (authors.Any()) AddAuthors(authors);
        if (categoryIds.Any()) AddCategories(categoryIds);
        if (beforeDate is not null) AddBeforeDate(beforeDate.Value);
        if (afterDate is not null) AddAfterDate(afterDate.Value);
    }

    public DocumentSearchParameters AddSources(List<long> sourceIds)
    {
        Parameters.Add(new QueryParameter(DocumentMap.SourceId, sourceIds));
        return this;
    }

    public DocumentSearchParameters AddAuthors(List<string> authorName)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Author, authorName));
        return this;
    }

    public DocumentSearchParameters AddCategories(List<int> categoryIds)
    {
        Parameters.Add(new QueryParameter(DocumentMap.CategoryId, categoryIds));
        return this;
    }

    public DocumentSearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Date, date, "<=", "beforeDate"));
        return this;
    }

    public DocumentSearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(DocumentMap.Date, date, ">=", "afterDate"));
        return this;
    }
}

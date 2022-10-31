using DocumentDataAPI.Data.Mappers;

namespace DocumentDataAPI.Models;

public class DocumentSearchParameters : ISearchParameters
{
    public List<QueryParameter> Parameters { get; }
    public DocumentSearchParameters()
    {
        Parameters = new();
    }

    public DocumentSearchParameters AddSource(int sourceId)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().MappedColumnName(nameof(DocumentModel.SourceId)), sourceId));
        return this;
    }

    public DocumentSearchParameters AddAuthor(string authorName)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().MappedColumnName(nameof(DocumentModel.Author)), authorName));
        return this;
    }

    public DocumentSearchParameters AddCategory(int categoryId)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().MappedColumnName(nameof(DocumentModel.CategoryId)), categoryId));
        return this;
    }

    public DocumentSearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().MappedColumnName(nameof(DocumentModel.Date)), date, "<="));
        return this;
    }

    public DocumentSearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().MappedColumnName(nameof(DocumentModel.Date)), date, ">="));
        return this;
    }
}

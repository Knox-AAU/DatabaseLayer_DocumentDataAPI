using DocumentDataAPI.Data.Mappers;

namespace DocumentDataAPI.Models;

public class DocumentSearchParameters : ISearchParameters
{
    public List<QueryParameter> Parameters { get; }
    public DocumentSearchParameters()
    {
        Parameters = new();
    }

    public DocumentSearchParameters AddDataSource(int dataSourceId)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().GetMappedColumnName(nameof(DocumentModel.DataSourceId)), dataSourceId));
        return this;
    }

    public DocumentSearchParameters AddAuthor(string authorName)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().GetMappedColumnName(nameof(DocumentModel.Author)), authorName));
        return this;
    }

    public DocumentSearchParameters AddCategory(int categoryId)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().GetMappedColumnName(nameof(DocumentModel.CategoryId)), categoryId));
        return this;
    }

    public DocumentSearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().GetMappedColumnName(nameof(DocumentModel.Date)), date, "<="));
        return this;
    }

    public DocumentSearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter(new DocumentMap().GetMappedColumnName(nameof(DocumentModel.Date)), date, ">="));
        return this;
    }
}

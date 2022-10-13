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
        Parameters.Add(new QueryParameter("sources_id", sourceId));
        return this;
    }

    public DocumentSearchParameters AddAuthor(string authorName)
    {
        Parameters.Add(new QueryParameter("author", authorName));
        return this;
    }

    public DocumentSearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter("date", date, "<="));
        return this;
    }

    public DocumentSearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter("date", date, ">="));
        return this;
    }

}

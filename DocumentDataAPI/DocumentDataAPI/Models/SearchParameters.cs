namespace DocumentDataAPI.Models;

public class SearchParameters
{
    public readonly List<QueryParameter> Parameters = new();

    public SearchParameters()
    {
    }

    public SearchParameters AddSource(int sourceId)
    {
        Parameters.Add(new QueryParameter("sources_id", sourceId));
        return this;
    }

    public SearchParameters AddAuthor(string authorName)
    {
        Parameters.Add(new QueryParameter("author", authorName));
        return this;
    }

    public SearchParameters AddBeforeDate(DateTime date)
    {
        Parameters.Add(new QueryParameter("date", date, "<="));
        return this;
    }

    public SearchParameters AddAfterDate(DateTime date)
    {
        Parameters.Add(new QueryParameter("date", date, ">="));
        return this;
    }
}
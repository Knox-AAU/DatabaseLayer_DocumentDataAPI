using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class DocumentMap : EntityMap<DocumentModel>
{
    public const string Id = "id";
    public const string SourceId = "sources_id";
    public const string Title = "title";
    public const string Path = "path";
    public const string Summary = "summary";
    public const string Date = "date";
    public const string Author = "author";
    public const string TotalWords = "total_words";
    public const string CategoryId = "categories_id";
    public const string Publication = "publication";
    public const string UniqueWords = "unique_words";

    public DocumentMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.SourceId).ToColumn(SourceId);
        Map(x => x.Title).ToColumn(Title);
        Map(x => x.Path).ToColumn(Path);
        Map(x => x.Summary).ToColumn(Summary);
        Map(x => x.Date).ToColumn(Date);
        Map(x => x.Author).ToColumn(Author);
        Map(x => x.TotalWords).ToColumn(TotalWords);
        Map(x => x.CategoryId).ToColumn(CategoryId);
        Map(x => x.Publication).ToColumn(Publication);
        Map(x => x.UniqueWords).ToColumn(UniqueWords);
    }
}

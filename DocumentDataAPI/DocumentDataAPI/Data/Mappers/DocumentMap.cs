using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class DocumentMap : EntityMap<DocumentModel>
{
    public DocumentMap()
    {
        Map(x => x.Id).ToColumn("id");
        Map(x => x.SourceId).ToColumn("sources_id");
        Map(x => x.Title).ToColumn("title");
        Map(x => x.Path).ToColumn("path");
        Map(x => x.Summary).ToColumn("summary");
        Map(x => x.Date).ToColumn("date");
        Map(x => x.Author).ToColumn("author");
        Map(x => x.TotalWords).ToColumn("total_words");
        Map(x => x.CategoryId).ToColumn("categories_id");
        Map(x => x.Publication).ToColumn("publication");
        Map(x => x.UniqueWords).ToColumn("unique_words");
    }
}

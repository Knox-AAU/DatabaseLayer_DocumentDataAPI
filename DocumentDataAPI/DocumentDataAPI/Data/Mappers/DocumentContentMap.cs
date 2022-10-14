using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class DocumentContentMap : EntityMap<DocumentContentModel>
{
    public DocumentContentMap()
    {
        Map(x => x.DocumentId).ToColumn("documents_id");
        Map(x => x.Content).ToColumn("content");
    }
}
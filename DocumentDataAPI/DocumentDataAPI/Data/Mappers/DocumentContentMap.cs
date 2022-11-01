using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class DocumentContentMap : EntityMap<DocumentContentModel>
{
    public const string DocumentId = "documents_id";
    public const string Content = "content";
    public const string Subheading = "subheading";
    public const string Index = "index";

    public DocumentContentMap()
    {
        Map(x => x.DocumentId).ToColumn(DocumentId);
        Map(x => x.Content).ToColumn(Content);
        Map(x => x.Subheading).ToColumn(Subheading);
        Map(x => x.Index).ToColumn(Index);
    }
}

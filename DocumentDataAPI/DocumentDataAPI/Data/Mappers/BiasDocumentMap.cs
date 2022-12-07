using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class BiasDocumentMap : EntityMap<BiasDocumentModel>
{
    public const string Id = "id";
    public const string PartyId = "party_id";
    public const string Document = "document";
    public const string DocumentLemmatized = "document_lemmatized";

    public BiasDocumentMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.PartyId).ToColumn(PartyId);
        Map(x => x.Document).ToColumn(Document);
        Map(x => x.DocumentLemmatized).ToColumn(DocumentLemmatized);
    }
}

using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class SimilarDocumentMap : EntityMap<SimilarDocumentModel>
{
    public SimilarDocumentMap()
    {
        Map(x => x.Similarity).ToColumn("similarity");
        Map(x => x.MainDocumentId).ToColumn("main_document_id");
        Map(x => x.SimilarDocumentId).ToColumn("similar_document_id");
    }
}

using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class SimilarDocumentMap : EntityMap<SimilarDocumentModel>
{
    public const string Similarity = "similarity";
    public const string MainDocumentId = "main_document_id";
    public const string SimilarDocumentId = "similar_document_id";

    public SimilarDocumentMap()
    {
        Map(x => x.Similarity).ToColumn(Similarity);
        Map(x => x.MainDocumentId).ToColumn(MainDocumentId);
        Map(x => x.SimilarDocumentId).ToColumn(SimilarDocumentId);
    }
}

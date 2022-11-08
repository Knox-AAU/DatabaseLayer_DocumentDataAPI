using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class WordRatioMap : EntityMap<WordRatioModel>
{
    public const string Amount = "amount";
    public const string ClusteringScore = "clustering_score";
    public const string DocumentId = "documents_id";
    public const string Percent = "percent";
    public const string Rank = "rank";
    public const string TfIdf = "tf-idf";
    public const string Word = "word";

    public WordRatioMap()
    {
        Map(x => x.DocumentId).ToColumn(DocumentId);
        Map(x => x.Word).ToColumn(Word);
        Map(x => x.Amount).ToColumn(Amount);
        Map(x => x.Percent).ToColumn(Percent);
        Map(x => x.Rank).ToColumn(Rank);
        Map(x => x.ClusteringScore).ToColumn(ClusteringScore);
        Map(x => x.TfIdf).ToColumn(TfIdf);
    }
}

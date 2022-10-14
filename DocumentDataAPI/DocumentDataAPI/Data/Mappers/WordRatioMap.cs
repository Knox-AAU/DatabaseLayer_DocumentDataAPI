using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class WordRatioMap : EntityMap<WordRatioModel>
{
    public WordRatioMap()
    {
        Map(x => x.DocumentId).ToColumn("documents_id");
        Map(x => x.Word).ToColumn("word");
        Map(x => x.Amount).ToColumn("amount");
        Map(x => x.Percent).ToColumn("percent");
        Map(x => x.Rank).ToColumn("rank");
    }
}

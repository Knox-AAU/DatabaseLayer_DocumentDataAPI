using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Mappers.BiasSchema;

public class BiasWordCountMap : EntityMap<BiasWordCountModel>
{
    public const string Id = "id";
    public const string Word = "word";
    public const string Count = "count";
    public const string WordFrequency = "word_frequency";

    public BiasWordCountMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.Word).ToColumn(Word);
        Map(x => x.Count).ToColumn(Count);
        Map(x => x.WordFrequency).ToColumn(WordFrequency);
    }
}
using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Mappers.BiasSchema;

public class BiasPoliticalPartiesMap : EntityMap<BiasPoliticalPartiesModel>
{
    public const string Id = "id";
    public const string PartyName = "party_name";
    public const string PartyBias = "party_bias";

    public BiasPoliticalPartiesMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.PartyName).ToColumn(PartyName);
        Map(x => x.PartyBias).ToColumn(PartyBias);
    }
}
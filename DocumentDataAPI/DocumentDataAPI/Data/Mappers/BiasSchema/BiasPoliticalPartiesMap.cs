using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Mappers.BiasSchema;

public class BiasPoliticalPartiesMap : EntityMap<BiasPoliticalPartiesModel>
{
    public const string Id = "id";
    public const string Name = "name";
    public const string Bias = "bias";

    public BiasPoliticalPartiesMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.Name).ToColumn(Name);
        Map(x => x.Bias).ToColumn(Bias);
    }
}
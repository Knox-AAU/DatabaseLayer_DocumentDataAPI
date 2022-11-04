using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class SourceMap : EntityMap<SourceModel>
{
    public const string Id = "id";
    public const string Name = "name";

    public SourceMap()
    {
        Map(x => x.Id).ToColumn(Id);
        Map(x => x.Name).ToColumn(Name);
    }
}

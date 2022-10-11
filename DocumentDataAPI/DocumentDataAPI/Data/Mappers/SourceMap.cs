using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class SourceMap : EntityMap<SourceModel>
{
    public SourceMap()
    {
        Map(x => x.Id).ToColumn("id");
        Map(x => x.Name).ToColumn("name");
    }
}

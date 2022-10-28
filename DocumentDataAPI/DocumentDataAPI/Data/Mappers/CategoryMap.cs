using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class CategoryMap : EntityMap<CategoryModel>
{
    public CategoryMap()
    {
        Map(x => x.Name).ToColumn("name");
        Map(x => x.Id).ToColumn("id");
    }
}

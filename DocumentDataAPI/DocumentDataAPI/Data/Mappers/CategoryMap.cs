using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Mappers;

public class CategoryMap : EntityMap<CategoryModel>
{
    public const string Id = "id";
    public const string Name = "name";

    public CategoryMap()
    {
        Map(x => x.Name).ToColumn(Name);
        Map(x => x.Id).ToColumn(Id);
    }
}

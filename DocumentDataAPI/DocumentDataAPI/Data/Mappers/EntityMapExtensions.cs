using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Mapping;

namespace DocumentDataAPI.Data.Mappers;

public static class EntityMapExtensions
{
    public static string GetMappedColumnName<T>(this EntityMap<T> entityMap, string propertyName) where T : class
    {
        return entityMap.PropertyMaps.FirstOrDefault(x => x.PropertyInfo.Name == propertyName)?.ColumnName
               ?? throw new ArgumentException("There exists no mapping for the given property", nameof(propertyName));
    }
}

using Microsoft.Build.Framework;

namespace DocumentDataAPI.Models;

public class DataSourceModel
{
    public DataSourceModel()
    {
    }

    public DataSourceModel(long id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required]
    public long Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
}

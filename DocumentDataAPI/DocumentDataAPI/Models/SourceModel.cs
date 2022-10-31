using System.ComponentModel.DataAnnotations;

namespace DocumentDataAPI.Models;

public class SourceModel
{
    public SourceModel()
    {
    }

    public SourceModel(long id, string name)
    {
        Id = id;
        Name = name;
    }

    [Required]
    public long Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
}

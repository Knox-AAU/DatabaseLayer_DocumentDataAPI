using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models;

public class CategoryModel
{
    public CategoryModel()
    {

    }

    public CategoryModel(int id)
    {
        Id = id;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public int Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
}

using System.ComponentModel.DataAnnotations;

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
    public int Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
}

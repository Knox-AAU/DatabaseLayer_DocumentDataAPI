using Microsoft.Build.Framework;

namespace DocumentDataAPI.Models;

public class CategoryModel
{
    public CategoryModel()
    {

    }

    public CategoryModel(int categoryId)
    {
        CategoryId = categoryId;
    }


    [Required]
    public int CategoryId { get; init; }
    [Required]
    public string Name { get; init; } = null!;
}

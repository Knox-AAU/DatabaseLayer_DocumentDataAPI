using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models.BiasSchema;

public class BiasPoliticalPartiesModel
{
    public BiasPoliticalPartiesModel()
    {
    }

    public BiasPoliticalPartiesModel(int id, string name, decimal[] bias)
    {
        Id = id;
        Name = name;
        Bias = bias;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public int Id { get; init; }
    [Required]
    public string Name { get; init; } = null!;
    [Required]
    public decimal[] Bias { get; init; } = null!;
}
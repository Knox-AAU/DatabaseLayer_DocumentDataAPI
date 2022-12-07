using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models;

public class BiasPoliticalPartiesModel{
    public BiasPoliticalPartiesModel()
    {

    }

    public BiasPoliticalPartiesModel(int id, string partyName, float partyBias)
    {
        Id = id;
        PartyName = partyName;
        PartyBias = partyBias;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public int Id { get; init; }
    [Required]
    public string PartyName { get; init; } = null!;
    [Required]
    public float PartyBias { get; init; }
}
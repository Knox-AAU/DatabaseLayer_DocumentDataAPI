using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models;

public class BiasPoliticalPartiesModel{
    public BiasPoliticalPartiesModel()
    {

    }

    public BiasPoliticalPartiesModel(long id, string partyName, float partyBias)
    {
        Id = id;
        PartyName = partyName;
        PartyBias = partyBias;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public long Id { get; init; }
    [Required]
    public string PartyName { get; init; } = null!;
    [Required]
    public float PartyBias { get; init; }
}
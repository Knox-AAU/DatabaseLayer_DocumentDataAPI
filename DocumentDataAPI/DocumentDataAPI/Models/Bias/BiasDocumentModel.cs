using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;

namespace DocumentDataAPI.Models;

public class BiasDocumentModel
{
    public BiasDocumentModel()
    {
    }

    public BiasDocumentModel(long id, int partyId, string document, string? documentLemmatized, string? url)
    {
        Id = id;
        PartyId = partyId;
        Document = document;
        DocumentLemmatized = documentLemmatized;
        Url = url;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public long Id { get; init; }
    [Required]
    public int PartyId { get; init; }
    [Required]
    public string Document { get; init; } = null!;
    public string? DocumentLemmatized { get; init; }
    public string? Url { get; init; }
}
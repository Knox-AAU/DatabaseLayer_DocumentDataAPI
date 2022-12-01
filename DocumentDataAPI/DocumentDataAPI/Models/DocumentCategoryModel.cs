using System.ComponentModel.DataAnnotations;
using DocumentDataAPI.Models.Attributes;
using Microsoft.CodeAnalysis;

namespace DocumentDataAPI.Models;

public class DocumentCategoryModel
{
    public DocumentCategoryModel(long documentId, int categoryId)
    {
        DocumentId = documentId;
        CategoryId = categoryId;
    }

    [Required]
    [ExcludeFromGeneratedInsertStatement]
    public long DocumentId { get; init; }
    [Required]
    public int CategoryId { get; init; }
}

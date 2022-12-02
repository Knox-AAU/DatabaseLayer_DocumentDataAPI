using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    Task<DocumentModel?> Get(long documentId);
    Task<int> Delete(long documentId);
    Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters, int? limit = null, int? offset = null);
    Task<int> GetTotalDocumentCount();
    Task<IEnumerable<long>> AddBatch(List<DocumentModel> models);
    Task<IEnumerable<string>> GetAuthors();
    Task<int> Update(DocumentModel entity);
    Task<int> UpdateCategory(DocumentCategoryModel entity);
    Task<long> Add(DocumentModel entity);
}

using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    Task<DocumentModel?> Get(long documentId);
    Task<int> Delete(long documentId);
    Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters);
    Task<int> GetTotalDocumentCount();
    Task<int> AddBatch(List<DocumentModel> models);
}

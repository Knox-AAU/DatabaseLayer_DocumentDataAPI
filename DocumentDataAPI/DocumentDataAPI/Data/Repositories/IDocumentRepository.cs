using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    Task<DocumentModel?> Get(long id);
    Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters);
    Task<int> GetTotalDocumentCount();
    Task<IEnumerable<long>> AddBatch(List<DocumentModel> models);
}

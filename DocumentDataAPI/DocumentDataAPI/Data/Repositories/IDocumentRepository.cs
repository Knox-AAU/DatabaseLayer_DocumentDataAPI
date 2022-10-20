using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    DocumentModel? Get(long id);
    IEnumerable<DocumentModel> GetAll(DocumentSearchParameters parameters);
    int GetTotalDocumentCount();
    int AddBatch(List<DocumentModel> models);
}

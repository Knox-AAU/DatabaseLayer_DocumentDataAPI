using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    DocumentModel? Get(long id);
    IEnumerable<DocumentModel> GetAll(DocumentSearchParameters parameters);
    IEnumerable<DocumentModel> GetByAuthor(string author);
    IEnumerable<DocumentModel> GetByDate(DateTime dateTime);
    IEnumerable<DocumentModel> GetBySource(int id);
    int GetTotalDocumentCount();
    int AddBatch(List<DocumentModel> models);
}

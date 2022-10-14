using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    public Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters);
    public Task<IEnumerable<DocumentModel>> GetByAuthor(string author);
    public Task<IEnumerable<DocumentModel>> GetByDate(DateTime dateTime);
    public Task<IEnumerable<DocumentModel>> GetBySource(int id);
    public Task<int> GetTotalDocumentCount();
}

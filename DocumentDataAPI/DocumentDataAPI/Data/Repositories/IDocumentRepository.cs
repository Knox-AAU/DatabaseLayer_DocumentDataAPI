using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    public IEnumerable<DocumentModel> GetAll(DocumentSearchParameters parameters);
    public IEnumerable<DocumentModel> GetByAuthor(string author);
    public IEnumerable<DocumentModel> GetByDate(DateTime dateTime);
    public IEnumerable<DocumentModel> GetBySource(int id);
    public int GetTotalDocumentCount();
}

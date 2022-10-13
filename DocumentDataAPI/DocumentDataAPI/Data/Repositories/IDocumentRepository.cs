using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    IEnumerable<DocumentModel> GetByAuthor(string author);
    IEnumerable<DocumentModel> GetByDate(DateTime dateTime);
    IEnumerable<DocumentModel> GetBySource(int id);
    int GetTotalDocumentCount();
}

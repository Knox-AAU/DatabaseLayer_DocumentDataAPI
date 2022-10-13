using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentRepository : IRepository<DocumentModel>
{
    public IEnumerable<DocumentModel> GetAll(DocumentSearchParameters parameters);
}

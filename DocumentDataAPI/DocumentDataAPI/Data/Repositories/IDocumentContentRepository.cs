using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentContentRepository : IRepository<DocumentContentModel>
{
    DocumentContentModel? Get(long id);
}
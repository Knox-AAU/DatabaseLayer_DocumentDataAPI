using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentContentRepository : IRepository<DocumentContentModel>
{
    Task<DocumentContentModel?> Get(long id);
    Task<int> AddBatch(List<DocumentContentModel> models);
}

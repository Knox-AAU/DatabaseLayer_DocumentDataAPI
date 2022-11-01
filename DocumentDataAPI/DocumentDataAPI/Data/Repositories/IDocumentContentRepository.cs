using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentContentRepository : IRepository<DocumentContentModel>
{
    Task<DocumentContentModel?> Get(long id, int index);
    Task<int> Delete(long id, int index);
    Task<int> AddBatch(List<DocumentContentModel> models);
}

using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentContentRepository : IRepository<DocumentContentModel>
{
    Task<DocumentContentModel?> Get(long documentId, int index);
    Task<int> Delete(long documentId, int index);
    Task<int> AddBatch(List<DocumentContentModel> models);
}

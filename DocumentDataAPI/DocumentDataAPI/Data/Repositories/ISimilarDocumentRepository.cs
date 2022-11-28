using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISimilarDocumentRepository : IRepository<SimilarDocumentModel>
{
    Task<SimilarDocumentModel?> Get(long mainDocumentId, long similarDocumentId);
    Task<int> Delete(long mainDocumentId, long similarDocumentId);
    Task<IEnumerable<long>> AddBatch(List<SimilarDocumentModel> models);
}

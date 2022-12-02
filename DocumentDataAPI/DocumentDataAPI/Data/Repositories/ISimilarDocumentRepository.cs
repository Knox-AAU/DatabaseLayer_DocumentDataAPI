using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISimilarDocumentRepository : IRepository<SimilarDocumentModel>
{
    Task<IEnumerable<SimilarDocumentModel>> Get(long mainDocumentId);
    Task<int> DeleteAll();
    Task<IEnumerable<int>> AddBatch(List<SimilarDocumentModel> models);
}

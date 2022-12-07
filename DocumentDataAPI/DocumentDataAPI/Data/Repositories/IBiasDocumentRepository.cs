using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IBiasDocumentRepository : IRepository<BiasDocumentModel>
{
    Task<int> Delete(long documentId);
    Task<int> Update(BiasDocumentModel entity);
    Task<long> Add(BiasDocumentModel entity);
    Task<IEnumerable<long>> AddBatch(List<BiasDocumentModel> models);
    Task<BiasDocumentModel?> Get(long documentId);
}
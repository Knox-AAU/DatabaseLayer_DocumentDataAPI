using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Repositories.BiasSchema;

public interface IBiasDocumentRepository : IRepository<BiasDocumentModel>
{
    Task<int> Delete(long documentId);
    Task<int> Update(BiasDocumentModel entity);
    Task<long> Add(BiasDocumentModel entity);
    Task<IEnumerable<long>> AddBatch(List<BiasDocumentModel> models);
    Task<BiasDocumentModel?> Get(long documentId);
}
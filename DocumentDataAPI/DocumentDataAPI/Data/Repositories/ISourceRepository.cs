using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    Task<SourceModel?> Get(long sourceId);
    Task<int> Delete(long sourceId);
    Task<long> GetCountFromId(long id);
    Task<IEnumerable<SourceModel>> GetByName(string name);
    Task<int> Update(SourceModel entity);
    Task<long> Add(SourceModel entity);
}

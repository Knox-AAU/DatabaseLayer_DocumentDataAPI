using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    Task<SourceModel?> Get(long id);
    Task<long> GetCountFromId(long id);
    Task<IEnumerable<SourceModel>> GetByName(string name);
}

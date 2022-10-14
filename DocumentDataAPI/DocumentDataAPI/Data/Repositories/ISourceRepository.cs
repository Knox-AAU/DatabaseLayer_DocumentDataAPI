using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    public Task<long> GetCountFromId(long id);
    public IEnumerable<SourceModel> GetByName(string name);
}

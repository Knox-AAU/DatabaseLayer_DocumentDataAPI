using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    public long GetCountFromId(long id);
    public IEnumerable<SourceModel> GetByName(string name);
}

using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    SourceModel? Get(long id);
    long GetCountFromId(long id);
    IEnumerable<SourceModel> GetByName(string name);
}

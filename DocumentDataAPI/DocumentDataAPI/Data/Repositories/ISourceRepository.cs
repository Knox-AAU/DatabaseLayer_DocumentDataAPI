using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    public int GetCountFromId(int id);
}
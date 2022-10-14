using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    SourceModel Get(long id);
    new IEnumerable<SourceModel> GetAll();
    new int Add(SourceModel entity);
    new int Delete(SourceModel entity);
    new int Update(SourceModel entity);
    public long GetCountFromId(long id);
    public IEnumerable<SourceModel> GetByName(string name);
}
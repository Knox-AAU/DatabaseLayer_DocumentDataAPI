using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    SourceModel Get(int id);
    new IEnumerable<SourceModel> GetAll();
    new int Add(SourceModel entity);
    new int Delete(SourceModel entity);
    new int Update(SourceModel entity);
    int GetCountFromId(int id);
}

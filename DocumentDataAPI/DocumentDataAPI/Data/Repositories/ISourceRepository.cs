using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository : IRepository<SourceModel>
{
    SourceModel Get(int id);
    IEnumerable<SourceModel> GetAll();
    int Add(SourceModel entity);
    int Delete(SourceModel entity);
    int Update(SourceModel entity);
    int GetCountFromId(int id);
}

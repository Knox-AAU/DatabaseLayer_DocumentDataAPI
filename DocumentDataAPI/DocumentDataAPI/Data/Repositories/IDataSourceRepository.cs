using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDataSourceRepository : IRepository<DataSourceModel>
{
    Task<DataSourceModel?> Get(long id);
    Task<long> GetCountFromId(long id);
    Task<IEnumerable<DataSourceModel>> GetByName(string name);
}

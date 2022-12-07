using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IBiasWordCountRepository : IRepository<BiasWordCountModel>
{
    Task<IEnumerable<int>> AddBatch(List<BiasWordCountModel> models);
    Task<int> DeleteAll();
}
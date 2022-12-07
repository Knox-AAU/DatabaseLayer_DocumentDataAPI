using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IBiasWordCountRepository : IRepository<BiasWordCountModel>
{
    Task<IEnumerable<long>> AddBatch(List<BiasWordCountModel> models);
    Task<long> DeleteAll();
}
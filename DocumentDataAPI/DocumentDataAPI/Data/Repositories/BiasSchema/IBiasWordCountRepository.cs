using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Repositories.BiasSchema;

public interface IBiasWordCountRepository : IRepository<BiasWordCountModel>
{
    Task<IEnumerable<long>> AddBatch(List<BiasWordCountModel> models);
    Task<long> DeleteAll();
}
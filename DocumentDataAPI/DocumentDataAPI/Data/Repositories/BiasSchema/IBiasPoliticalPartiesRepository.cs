using DocumentDataAPI.Models.BiasSchema;

namespace DocumentDataAPI.Data.Repositories.BiasSchema;

public interface IBiasPoliticalPartiesRepository : IRepository<BiasPoliticalPartiesModel>
{
    Task<BiasPoliticalPartiesModel?> Get(int partyId);
    Task<int> Delete(int Id);
    Task<int> Update(BiasPoliticalPartiesModel entity);
    Task<IEnumerable<long>> AddBatch(List<BiasPoliticalPartiesModel> models);
    Task<int> Add(BiasPoliticalPartiesModel entity);

}
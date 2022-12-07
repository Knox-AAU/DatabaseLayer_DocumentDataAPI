using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IBiasPoliticalPartiesRepository : IRepository<BiasPoliticalPartiesModel>
{
    Task<DocumentModel?> Get(int partyId);
    Task<int> Delete(int Id);
    Task<int> Update(BiasPoliticalPartiesModel entity);
    Task<IEnumerable<long>> AddBatch(List<BiasPoliticalPartiesModel> models);
    Task<int> Add(BiasPoliticalPartiesModel entity);

}
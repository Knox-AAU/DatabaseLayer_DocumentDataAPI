using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IBiasPoliticalPartiesRepository : IRepository<BiasPoliticalPartiesModel>
{
    Task<int> Delete(int Id);
    Task<int> Update(BiasPoliticalPartiesModel entity);
    Task<int> Add(BiasPoliticalPartiesModel entity);
}
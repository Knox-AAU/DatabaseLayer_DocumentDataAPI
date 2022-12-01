using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ICategoryRepository : IRepository<CategoryModel>
{
    Task<CategoryModel?> Get(int id);
    Task<int> Delete(int id);
    Task<int> Update(CategoryModel entity);
    Task<long> Add(CategoryModel entity);
}

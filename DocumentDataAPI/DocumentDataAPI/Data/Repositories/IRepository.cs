namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll(int? limit = null, int? offset = null);
    Task<long> Add(TEntity entity);
    Task<int> Update(TEntity entity);
}

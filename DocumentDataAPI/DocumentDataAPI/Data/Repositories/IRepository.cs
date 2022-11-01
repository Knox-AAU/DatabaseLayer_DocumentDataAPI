namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<long> Add(TEntity entity);
    Task<int> Delete(TEntity entity);
    Task<int> Update(TEntity entity);
}

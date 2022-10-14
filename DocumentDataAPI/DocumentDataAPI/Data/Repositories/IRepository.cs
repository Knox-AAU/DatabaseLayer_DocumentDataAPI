namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    public Task<TEntity?> Get(long id);
    public Task<IEnumerable<TEntity>> GetAll();
    public Task<int> Add(TEntity entity);
    public Task<int> Delete(TEntity entity);
    public Task<int> Update(TEntity entity);
}

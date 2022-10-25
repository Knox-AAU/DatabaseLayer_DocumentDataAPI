namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    public Task<IEnumerable<TEntity>> GetAll();
    public Task<int> Add(TEntity entity);
    public Task<int> Delete(TEntity entity);
    public Task<int> Update(TEntity entity);
}

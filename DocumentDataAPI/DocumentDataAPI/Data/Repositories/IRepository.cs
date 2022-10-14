namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    public IEnumerable<TEntity> GetAll();
    public int Add(TEntity entity);
    public int Delete(TEntity entity);
    public int Update(TEntity entity);
}

using System.Collections.Generic;

namespace DocumentDataAPI.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    public TEntity Get(int id);
    public IEnumerable<TEntity> GetAll();
    public int Add(TEntity entity);
    public void Delete(TEntity entity);
    public void Update(TEntity entity);
}

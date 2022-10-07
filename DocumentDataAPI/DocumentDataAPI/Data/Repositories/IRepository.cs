using System.Collections.Generic;

namespace WordCount.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    public TEntity Get(int id);
    public IEnumerable<TEntity> GetAll();
    public void Add(TEntity entity);
    public void Delete(TEntity entity);
    public void Update(TEntity entity);
}
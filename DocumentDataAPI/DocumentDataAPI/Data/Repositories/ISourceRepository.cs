using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISourceRepository
{
    SourceModel Get(int id);
    IEnumerable<SourceModel> GetAll();
    int Add(SourceModel entity);
    void Delete(SourceModel entity);
    void Update(SourceModel entity);
    int GetCountFromId(int id);
}

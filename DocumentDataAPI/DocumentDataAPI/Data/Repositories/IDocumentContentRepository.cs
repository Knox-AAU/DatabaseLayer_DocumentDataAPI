using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IDocumentContentRepository : IRepository<DocumentContentModel>
{
    int AddBatch(List<DocumentContentModel> models);
}

using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISearchRepository : IRepository<DocumentModel>
{
    Task<IEnumerable<SearchResponseModel>> Get(List<string> processedWords, DocumentSearchParameters parameters);
}

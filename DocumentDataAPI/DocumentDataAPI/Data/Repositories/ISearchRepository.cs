using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface ISearchRepository
{
    Task<IEnumerable<SearchResponseModel>> Get(List<string> processedWords, DocumentSearchParameters parameters, int? limit = null, int? offset = null);
}

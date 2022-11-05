using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSearchRepository : ISearchRepository
{
    public async Task<IEnumerable<SearchResponseModel>> Get(List<string> processedWords, DocumentSearchParameters parameters)
    {
        throw new NotImplementedException();
    }
}

using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSearchRepository : ISearchRepository
{
    public async Task<IEnumerable<DocumentModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<long> Add(DocumentModel entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Update(DocumentModel entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SearchResponseModel>> Get(List<string> processedWords, DocumentSearchParameters parameters)
    {
        throw new NotImplementedException();
    }
}

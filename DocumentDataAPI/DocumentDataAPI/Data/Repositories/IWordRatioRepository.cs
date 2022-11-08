using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    Task<WordRatioModel?> Get(long id, string word);
    Task<int> Delete(long documentId, string word);
    Task<IEnumerable<WordRatioModel>> GetByWord(string word);
    Task<IEnumerable<WordRatioModel>> GetByWords(IEnumerable<string> word);
    Task<IEnumerable<WordRatioModel>> GetByDocumentId(long id);
    Task<int> AddBatch(List<WordRatioModel> models);
}

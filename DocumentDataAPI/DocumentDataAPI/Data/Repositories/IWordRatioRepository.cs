using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    Task<WordRatioModel?> GetByDocumentIdAndWord(int id, string word);
    Task<IEnumerable<WordRatioModel>> GetByWord(string word);
    Task<IEnumerable<WordRatioModel>> GetByWords(IEnumerable<string> word);
    Task<IEnumerable<WordRatioModel>> GetByDocumentId(int id);
    Task<int> AddBatch(List<WordRatioModel> entities);
}

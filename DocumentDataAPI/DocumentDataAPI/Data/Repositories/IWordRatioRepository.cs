using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    WordRatioModel? GetByDocumentIdAndWord(int id, string word);
    IEnumerable<WordRatioModel> GetByWord(string word);
    IEnumerable<WordRatioModel> GetByWords(IEnumerable<string> word);
    IEnumerable<WordRatioModel> GetByDocumentId(int id);
    int AddBatch(List<WordRatioModel> entities);
}

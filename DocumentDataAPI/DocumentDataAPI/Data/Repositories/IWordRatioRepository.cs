using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    WordRatioModel? GetByDocumentIdAndWord(int id, string word);
    new IEnumerable<WordRatioModel> GetAll();
    new int Add(WordRatioModel entity);
    new int Delete(WordRatioModel entity);
    new int Update(WordRatioModel entity);
    IEnumerable<WordRatioModel> GetByWord(string word);
    IEnumerable<WordRatioModel> GetByWords(IEnumerable<string> word);
    int AddWordRatios(IEnumerable<WordRatioModel> entities);
}

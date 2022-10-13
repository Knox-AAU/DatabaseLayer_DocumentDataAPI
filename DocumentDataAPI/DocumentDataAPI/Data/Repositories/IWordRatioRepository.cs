using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    new WordRatioModel Get(int id);
    new IEnumerable<WordRatioModel> GetAll();
    new int Add(WordRatioModel entity);
    new int Delete(WordRatioModel entity);
    new int Update(WordRatioModel entity);
    IEnumerable<WordRatioModel> GetByWord(string word);
    IEnumerable<WordRatioModel> GetByWords(IEnumerable<string> word);
    int AddWordRatios(IEnumerable<WordRatioModel> entities);
    WordRatioModel GetByDocumentIdAndWord(int documentId, string word);
}

using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public interface IWordRatioRepository : IRepository<WordRatioModel>
{
    public WordRatioModel Get(int id);
    public IEnumerable<WordRatioModel> GetAll();
    public int Add(WordRatioModel entity);
    public int Delete(WordRatioModel entity);
    public int Update(WordRatioModel entity);
    public IEnumerable<WordRatioModel> GetByWord(string word);
    public IEnumerable<WordRatioModel> GetByWords(IEnumerable<string> word);
    public int AddWordRatios(IEnumerable<WordRatioModel> entities);
    public WordRatioModel GetByDocumentIDAndWord(int documentid, string word);
}

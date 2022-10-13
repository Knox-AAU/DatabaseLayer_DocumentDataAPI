using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class WordRatioRepository : IWordRatioRepository
{
  private readonly IDbConnectionFactory _connectionFactory;

  public WordRatioRepository(IDbConnectionFactory connectionFactory)
  {
    _connectionFactory = connectionFactory;
  }

  public int Add(WordRatioModel entity)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Execute(
        "insert into word_ratios(documents_id, word, amount, percent, rank)" +
        " values (@DocumentId, @Word, @Amount, @Percent, @Rank)",
        new
        {
          entity.DocumentId,
          entity.Word,
          entity.Amount,
          entity.Percent,
          entity.Rank
        });
  }

  public int AddWordRatios(IEnumerable<WordRatioModel> entities)
  {
    int rowsAffected = 0;
    using IDbConnection con = _connectionFactory.CreateConnection();
    IDbTransaction transaction = con.BeginTransaction();
    foreach (WordRatioModel entity in entities)
    {
      rowsAffected += con.Execute(
      "insert into word_ratios(documents_id, word, amount, percent, rank)" +
      " values (@DocumentId, @Word, @Amount, @Percent, @Rank)",
      new
      {
        entity.DocumentId,
        entity.Word,
        entity.Amount,
        entity.Percent,
        entity.Rank
      }, transaction: transaction);
    }
    if (rowsAffected != entities.Count())
    {
      transaction.Rollback();
      throw new RowsAffectedMismatchException();
    }
    transaction.Commit();
    return rowsAffected;
  }

  public int Delete(WordRatioModel entity)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Execute("delete from word_ratios where documents_id=@DocumentId", new { entity.DocumentId });
  }

  public WordRatioModel Get(int id)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.QuerySingle<WordRatioModel>("select * from word_ratios where id=@Id", new { id });
  }

  public IEnumerable<WordRatioModel> GetAll()
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Query<WordRatioModel>($"select * from word_ratios");
  }

  public WordRatioModel GetByDocumentIdAndWord(int documentId, string word)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.QuerySingle<WordRatioModel>(
        "select * from word_ratios where word = @Word and documents_id = @Documentid",
        new { DocumentId = documentId, Word = word });
  }

  public IEnumerable<WordRatioModel> GetByWord(string word)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Query<WordRatioModel>("select * from word_ratios where word = @Word", new { Word = word });
  }

  public IEnumerable<WordRatioModel> GetByWords(IEnumerable<string> wordlist)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Query<WordRatioModel>("select * from word_ratios where word = any(@wordlist)", new { wordlist });
  }

  public int Update(WordRatioModel entity)
  {
    using IDbConnection con = _connectionFactory.CreateConnection();
    return con.Execute(
        "update word_ratios set word = @Word, amount = @Amount, percent = @Percent, rank = @Rank where documents_id = @DocumentId",
        new
        {
          entity.Word,
          entity.Amount,
          entity.Percent,
          entity.Rank,
          entity.DocumentId
        });
  }
}

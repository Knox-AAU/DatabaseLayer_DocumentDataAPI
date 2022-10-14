using System.Data;
using Dapper;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgWordRatioRepository : IWordRatioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;

    public NpgWordRatioRepository(IDbConnectionFactory connectionFactory, ILogger<NpgWordRatioRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public IEnumerable<WordRatioModel> GetAll()
    {
        _logger.LogDebug("Retrieving all WordRatios from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<WordRatioModel>($"select * from word_ratios");
    }

    public int Add(WordRatioModel entity)
    {
        _logger.LogDebug("Adding WordRatio with id {DocumentId} to database", entity.DocumentId);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
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
        IEnumerable<WordRatioModel> wordRatios = entities.ToList();
        using IDbConnection con = _connectionFactory.CreateConnection();
        IDbTransaction transaction = con.BeginTransaction();
        foreach (WordRatioModel entity in wordRatios)
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

        if (rowsAffected != wordRatios.Count())
        {
            transaction.Rollback();
            throw new RowsAffectedMismatchException();
        }

        transaction.Commit();
        return rowsAffected;
    }

    public int Delete(WordRatioModel entity)
    {
        _logger.LogDebug("Deleting WordRatio with id {DocumentId} from database", entity.DocumentId);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from word_ratios " +
                           "where documents_id=@DocumentId", new { entity.DocumentId });
    }

    public int Update(WordRatioModel entity)
    {
        _logger.LogDebug("Updating WordRatio with id {DocumentId} in database", entity.DocumentId);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
            "update word_ratios set word = @Word, amount = @Amount, percent = @Percent, rank = @Rank " +
            "where documents_id = @DocumentId",
            new
            {
                entity.Word,
                entity.Amount,
                entity.Percent,
                entity.Rank,
                entity.DocumentId
            });
    }

    public WordRatioModel? GetByDocumentIdAndWord(int documentId, string word)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<WordRatioModel>(
            "select * from word_ratios where word = @Word and documents_id = @DocumentId",
            new { DocumentId = documentId, Word = word }).FirstOrDefault();
    }

    public IEnumerable<WordRatioModel> GetByDocumentId(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<WordRatioModel>("select * from word_ratios where documents_id = @DocumentId",
            new { DocumentId = id });
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
}

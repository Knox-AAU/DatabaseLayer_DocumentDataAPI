using System.Data;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgWordRatioRepository : IWordRatioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgWordRatioRepository(IDbConnectionFactory connectionFactory, ILogger<NpgWordRatioRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<IEnumerable<WordRatioModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all WordRatios from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>($"select * from word_ratios");
    }

    public async Task<long> Add(WordRatioModel entity)
    {
        _logger.LogDebug("Adding WordRatio with id {DocumentId} and name {Word} to database", entity.DocumentId,
            entity.Word);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "insert into word_ratios(documents_id, word, amount, percent, rank, clustering_score)" +
            "values (@DocumentId, @Word, @Amount, @Percent, @Rank, @ClusteringScore)",
            new
            {
                entity.DocumentId,
                entity.Word,
                entity.Amount,
                entity.Percent,
                entity.Rank,
                entity.ClusteringScore
            });
    }

    public async Task<int> AddBatch(List<WordRatioModel> models)
    {
        int rowsAffected = 0;
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (WordRatioModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                rowsAffected += await transaction.ExecuteAsync("insert into word_ratios(documents_id, word, amount, percent, rank, clustering_score) values " + parameterString, parameters);
            }

            if (rowsAffected != models.Count())
            {
                transaction.Rollback();
                throw new RowsAffectedMismatchException();
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert word ratios, rolling back transaction");
            throw;
        }
        return rowsAffected;
    }

    public async Task<int> Delete(WordRatioModel entity)
    {
        _logger.LogDebug("Deleting WordRatio with id {DocumentId} and word {Word} from database", entity.DocumentId,
            entity.Word);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "delete from word_ratios " +
            "where documents_id=@DocumentId and word=@Word", new { entity.DocumentId, entity.Word });
    }

    public async Task<int> Update(WordRatioModel entity)
    {
        _logger.LogDebug("Updating WordRatio with id {DocumentId} and word {Word} in database", entity.DocumentId,
            entity.Word);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "update word_ratios set amount = @Amount, percent = @Percent, rank = @Rank, clustering_score = @ClusteringScore " +
            "where documents_id = @DocumentId and word = @Word",
            new
            {
                entity.Word,
                entity.Amount,
                entity.Percent,
                entity.Rank,
                entity.DocumentId,
                entity.ClusteringScore
            });
    }

    public async Task<WordRatioModel?> GetByDocumentIdAndWord(int documentId, string word)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<WordRatioModel>(
            "select * from word_ratios where word = @Word and documents_id = @DocumentId",
            new { DocumentId = documentId, Word = word });
    }

    public async Task<IEnumerable<WordRatioModel>> GetByDocumentId(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>("select * from word_ratios where documents_id = @DocumentId",
            new { DocumentId = id });
    }

    public async Task<IEnumerable<WordRatioModel>> GetByWord(string word)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>("select * from word_ratios where word = @Word", new { Word = word });
    }

    public async Task<IEnumerable<WordRatioModel>> GetByWords(IEnumerable<string> wordlist)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>("select * from word_ratios where word = any(@wordlist)", new { wordlist });
    }
}

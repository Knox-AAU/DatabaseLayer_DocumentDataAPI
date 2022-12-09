using System.Data;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;

namespace DocumentDataAPI.Data.Repositories;

public class NpgWordRatioRepository : IWordRatioRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgWordRatioRepository(IDbConnectionFactory connectionFactory, ILogger<NpgWordRatioRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory.WithSchema(DatabaseOptions.Schema.DocumentData);
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<long> Add(WordRatioModel entity)
    {
        _logger.LogDebug("Adding WordRatio with id {DocumentId} and name {Word} to database", entity.DocumentId,
            entity.Word);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"insert into word_ratios({WordRatioMap.DocumentId}, {WordRatioMap.Word}, {WordRatioMap.Amount}, {WordRatioMap.Percent}, {WordRatioMap.Rank}, {WordRatioMap.ClusteringScore}) " +
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
                rowsAffected += await transaction.ExecuteAsync(
                    $"insert into word_ratios({WordRatioMap.DocumentId}, {WordRatioMap.Word}, {WordRatioMap.Amount}, {WordRatioMap.Percent}, {WordRatioMap.Rank}, {WordRatioMap.ClusteringScore}) values " + parameterString,
                    parameters);
            }

            if (rowsAffected != models.Count)
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

    public async Task<int> Delete(long documentId, string word)
    {
        _logger.LogDebug("Deleting WordRatio with id {DocumentId} and word {Word} from database", documentId,
            word);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"delete from word_ratios where {WordRatioMap.DocumentId} = @DocumentId and {WordRatioMap.Word} = @Word",
            new { documentId, word });
    }

    public async Task<WordRatioModel?> Get(long documentId, string word)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<WordRatioModel>(
            $"select * from word_ratios where {WordRatioMap.Word} = @Word and {WordRatioMap.DocumentId} = @DocumentId",
            new { DocumentId = documentId, Word = word });
    }

    public async Task<IEnumerable<WordRatioModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all WordRatios from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from word_ratios", limit, offset,
            WordRatioMap.DocumentId, WordRatioMap.Word);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>(sql);
    }

    public async Task<IEnumerable<WordRatioModel>> GetByDocumentId(long id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>($"select * from word_ratios where {WordRatioMap.DocumentId} = @DocumentId",
            new { DocumentId = id });
    }

    public async Task<IEnumerable<WordRatioModel>> GetByWord(string word)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>($"select * from word_ratios where {WordRatioMap.Word} = @Word",
            new { Word = word });
    }

    public async Task<IEnumerable<WordRatioModel>> GetByWords(IEnumerable<string> wordlist, int? limit = null, int? offset = null)
    {
        string sql = _sqlHelper.GetPaginatedQuery($"select * from word_ratios where {WordRatioMap.Word} = any(@wordlist)",
            limit, offset, WordRatioMap.DocumentId, WordRatioMap.Word);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>(sql, new { wordlist });
    }

    public async Task<int> Update(WordRatioModel entity)
    {
        _logger.LogDebug("Updating WordRatio with id {DocumentId} and word {Word} in database", entity.DocumentId,
            entity.Word);
        _logger.LogTrace("WordRatio: {WordRatio}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"update word_ratios set {WordRatioMap.Amount} = @Amount, {WordRatioMap.Percent} = @Percent, {WordRatioMap.Rank} = @Rank, {WordRatioMap.ClusteringScore} = @ClusteringScore " +
            $"where {WordRatioMap.DocumentId} = @DocumentId and {WordRatioMap.Word} = @Word",
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
}

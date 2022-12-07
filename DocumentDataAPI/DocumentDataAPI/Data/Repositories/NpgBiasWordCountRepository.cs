using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgBiasWordCountRepository : IBiasWordCountRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgBiasDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgBiasWordCountRepository(IDbConnectionFactory connectionFactory, ILogger<NpgBiasDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<IEnumerable<long>> AddBatch(List<BiasWordCountModel> models)
    {
        IEnumerable<long> results = new List<long>();
        _logger.LogDebug("Adding {count} entries to word_count table", models.Count);
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (BiasWordCountModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                results.Append(await con.ExecuteAsync(
                        $"insert into word_count({BiasWordCountMap.Id}, {BiasWordCountMap.Word}, {BiasWordCountMap.Count}, {BiasWordCountMap.WordFrequency}) " +
                        $"values {parameterString}",
                    parameters));
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert word_count entries, rolling back transaction");
            throw;
        }
        return results;
    }

    public async Task<long> DeleteAll()
    {
        _logger.LogDebug("Deleting all rows in word_count table");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from word_count where 1=1");
    }

    public async Task<IEnumerable<BiasWordCountModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all WordCount from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from word_count", limit, offset,
            BiasWordCountMap.Word);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<BiasWordCountModel>(sql);
    }
}
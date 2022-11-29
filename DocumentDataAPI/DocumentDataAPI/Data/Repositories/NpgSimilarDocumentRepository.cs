using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using System.Data;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSimilarDocumentRepository : ISimilarDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSimilarDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;
    public NpgSimilarDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSimilarDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<IEnumerable<long>> AddBatch(List<SimilarDocumentModel> models)
    {
        IEnumerable<long> allInsertedIds = new List<long>();
        _logger.LogDebug("Adding {count} SimilarDocument to database", models.Count);
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (SimilarDocumentModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                IEnumerable<long> insertedIds = await transaction.QueryAsync<long>(
                        $"insert into similar_documents({SimilarDocumentMap.MainDocumentId}, {SimilarDocumentMap.SimilarDocumentId}, {SimilarDocumentMap.Similarity}) " +
                        $"values {parameterString} returning {SimilarDocumentMap.MainDocumentId}",
                    parameters);
                allInsertedIds = allInsertedIds.Concat(insertedIds);
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert similar documents, rolling back transaction");
            throw;
        }
        return allInsertedIds;
    }

    public async Task<int> DeleteAll()
    {
        _logger.LogDebug("Deleting all similar document from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from similar_documents where 1=1");
    }

    public async Task<IEnumerable<SimilarDocumentModel>> Get(long mainDocumentId)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SimilarDocumentModel>($"select * from similar_documents " +
            $"where {SimilarDocumentMap.MainDocumentId} = @mainId", new { mainId = mainDocumentId });
    }

    public async Task<IEnumerable<SimilarDocumentModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Similar Documents from database");
        string sql = _sqlHelper.GetPaginatedQuery($"select * from similar_documents", limit, offset,
            SimilarDocumentMap.MainDocumentId, SimilarDocumentMap.SimilarDocumentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SimilarDocumentModel>(sql);
    }
}
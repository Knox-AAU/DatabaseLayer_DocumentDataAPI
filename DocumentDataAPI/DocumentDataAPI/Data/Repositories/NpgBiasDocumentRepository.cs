using System.Data;
using System.Text;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgBiasDocumentRepository : IBiasDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgBiasDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgBiasDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgBiasDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<long> Add(BiasDocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<long>(
            $"insert into documents ({BiasDocumentMap.PartyId}, {BiasDocumentMap.Document}, {BiasDocumentMap.DocumentLemmatized})" +
            $"values (@PartyId, @Document, @DocumentLemmatized) returning {BiasDocumentMap.Id}",
            new
            {
                entity.PartyId,
                entity.Document,
                entity.DocumentLemmatized
            });
    }

    public async Task<IEnumerable<long>> AddBatch(List<BiasDocumentModel> models)
    {
        IEnumerable<long> allInsertedIds = new List<long>();
        _logger.LogDebug("Adding {count} documents to database", models.Count);
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (BiasDocumentModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                IEnumerable<long> insertedIds = await transaction.QueryAsync<long>(
                    $"insert into documents ({BiasDocumentMap.PartyId}, {BiasDocumentMap.Document}, {BiasDocumentMap.DocumentLemmatized}) " +
                    $"values {parameterString} returning {BiasDocumentMap.Id}",
                    parameters);
                allInsertedIds = allInsertedIds.Concat(insertedIds);
            }

            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert documents");
            throw;
        }
        return allInsertedIds;
    }

    public async Task<int> Delete(long documentId)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", documentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from documents where {BiasDocumentMap.Id} = @Id",
            new { id = documentId });
    }

    public async Task<IEnumerable<BiasDocumentModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Documents from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from documents", limit, offset, BiasDocumentMap.Id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<BiasDocumentModel>(sql);
    }

    public async Task<int> Update(BiasDocumentModel entity)
    {
        _logger.LogDebug("Updating Document with id {Id} in database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"update documents set {BiasDocumentMap.Id} = @Id, {BiasDocumentMap.PartyId} = @PartyId, {BiasDocumentMap.Document} = @Document, {BiasDocumentMap.DocumentLemmatized} = @DocumentLemmatized, " +
            $"where {BiasDocumentMap.Id} = @Id",
                        new
                        {
                            entity.Id,
                            entity.PartyId,
                            entity.Document,
                            entity.DocumentLemmatized
                        });
    }

    public async Task<BiasDocumentModel?> Get(long documentId)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", documentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<BiasDocumentModel>($"select * from documents where {BiasDocumentMap.Id} = @Id", new { id = documentId });
    }
}
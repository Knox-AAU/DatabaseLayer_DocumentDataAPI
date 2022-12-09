using System.Data;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentContentRepository : IDocumentContentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgDocumentContentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentContentRepository> logger,
        ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory.WithSchema(DatabaseOptions.Schema.DocumentData);
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<DocumentContentModel?> Get(long documentId, int index)
    {
        _logger.LogDebug("Retrieving DocumentContent with id {id} from database", documentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentContentModel>(
            $"select * from document_contents where {DocumentContentMap.DocumentId} = @Id and {DocumentContentMap.Index} = @Index",
            new { id = documentId, index });
    }

    public async Task<int> Delete(long documentId, int index)
    {
        _logger.LogDebug("Deleting DocumentContent with id {DocumentId} and index {Index} from database", documentId, index);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"delete from document_contents where {DocumentContentMap.DocumentId} = @DocumentId and {DocumentContentMap.Index} = @Index",
            new { documentId, index });
    }

    public async Task<IEnumerable<DocumentContentModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all DocumentContents from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from document_contents", limit, offset,
            DocumentContentMap.DocumentId, DocumentContentMap.Index);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentContentModel>(sql);
    }

    public async Task<long> Add(DocumentContentModel entity)
    {
        _logger.LogDebug("Adding DocumentContent with id {DocumentId} to database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"insert into document_contents ({DocumentContentMap.DocumentId}, {DocumentContentMap.Content}, {DocumentContentMap.Index}, {DocumentContentMap.Subheading}) " +
            "values (@DocumentId, @Index, @Subheading, @Content)",
                        new
                        {
                            entity.DocumentId,
                            entity.Content,
                            entity.Index,
                            entity.Subheading
                        });
    }

    public async Task<int> Update(DocumentContentModel entity)
    {
        _logger.LogDebug("Updating DocumentContent with id {DocumentId} in database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"update document_contents set {DocumentContentMap.Content} = @Content, {DocumentContentMap.Subheading} = @Subheading " +
            $"where {DocumentContentMap.DocumentId} = @DocumentId and {DocumentContentMap.Index} = @Index",
                        new
                        {
                            entity.Content,
                            entity.Subheading,
                            entity.DocumentId,
                            entity.Index
                        });
    }

    public async Task<int> AddBatch(List<DocumentContentModel> models)
    {
        _logger.LogDebug("Inserting {count} DocumentContents in database", models.Count);
        int rowsAffected = 0;
        using IDbConnection connection = _connectionFactory.CreateConnection();
        connection.Open();
        using IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (DocumentContentModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                rowsAffected += await transaction.ExecuteAsync(
                    $"insert into document_contents({DocumentContentMap.DocumentId}, {DocumentContentMap.Index}, {DocumentContentMap.Subheading}, {DocumentContentMap.Content}) values " + parameterString, parameters);
            }

            if (rowsAffected != models.Count)
            {
                throw new RowsAffectedMismatchException();
            }
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert document contents, rolling back transaction");
            throw;
        }
        return rowsAffected;
    }
}

using System.Data;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentContentRepository : IDocumentContentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgDocumentContentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentContentRepository> logger,
        ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public DocumentContentModel? Get(long id)
    {
        _logger.LogDebug("Retrieving DocumentContent with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QueryFirstOrDefault<DocumentContentModel>("select * from document_contents where documents_id=@Id", new { id });
    }

    public IEnumerable<DocumentContentModel> GetAll()
    {
        _logger.LogDebug("Retrieving all DocumentContents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentContentModel>("select * from document_contents");
    }

    public int Add(DocumentContentModel entity)
    {
        _logger.LogDebug("Adding DocumentContent with id {DocumentId} to database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("insert into document_contents(documents_id, content)" +
                        " values (@DocumentId, @Content)",
                        new
                        {
                            entity.DocumentId,
                            entity.Content
                        });
    }

    public int Delete(DocumentContentModel entity)
    {
        _logger.LogDebug("Deleting DocumentContent with id {DocumentId} from database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from document_contents " +
                           "where documents_id=@DocumentId", new { entity.DocumentId });
    }

    public int Update(DocumentContentModel entity)
    {
        _logger.LogDebug("Updating DocumentContent with id {DocumentId} in database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("update document_contents set content = @Content " +
                        "where documents_id = @DocumentId",
                        new
                        {
                            entity.Content,
                            entity.DocumentId
                        });
    }

    public int AddBatch(List<DocumentContentModel> models)
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
                rowsAffected += transaction.Execute(
                    "insert into document_contents (documents_id, content) values " + parameterString, parameters);
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

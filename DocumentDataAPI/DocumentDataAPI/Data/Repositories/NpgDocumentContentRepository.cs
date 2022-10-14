using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentContentRepository : IDocumentContentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;

    public NpgDocumentContentRepository(IDbConnectionFactory connectionFactory,
        ILogger<NpgDocumentContentRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<DocumentContentModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving DocumentContent with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentContentModel>("select * from document_contents where documents_id=@Id", new { id });
    }

    public async Task<IEnumerable<DocumentContentModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all DocumentContents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentContentModel>("select * from document_contents");
    }

    public async Task<int> Add(DocumentContentModel entity)
    {
        _logger.LogDebug("Adding DocumentContent with id {DocumentId} to database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
                        "insert into document_contents(documents_id, content)" +
                        " values (@DocumentId, @Content)",
                        new
                        {
                            entity.DocumentId,
                            entity.Content
                        });
    }

    public async Task<int> Delete(DocumentContentModel entity)
    {
        _logger.LogDebug("Deleting DocumentContent with id {DocumentId} from database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync("delete from document_contents " +
                                      "where documents_id=@DocumentId", new { entity.DocumentId });
    }

    public async Task<int> Update(DocumentContentModel entity)
    {
        _logger.LogDebug("Updating DocumentContent with id {DocumentId} in database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
                        "update document_contents set content = @Content " +
                        "where documents_id = @DocumentId",
                        new
                        {
                            entity.Content,
                            entity.DocumentId
                        });
    }
}

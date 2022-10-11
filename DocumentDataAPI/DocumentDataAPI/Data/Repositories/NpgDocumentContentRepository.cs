using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentContentRepository : IDocumentContentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;

    public NpgDocumentContentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentContentRepository> logger) {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public DocumentContentModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Retrieving DocumentContent with id {id} from database", id);
        return con.QuerySingle<DocumentContentModel>("select * from document_contents where documents_id=@Id", new{ id });
    }

    public IEnumerable<DocumentContentModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Retrieving all DocumentContents from database");
        return con.Query<DocumentContentModel>($"select * from document_contents");
    }

    public int Add(DocumentContentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Adding DocumentContent with id {DocumentId} to database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        return con.Execute(
            "insert into document_contents(documents_id, content)" +
                    " values (@DocumentId, @Content)",
                new
                {
                    entity.DocumentId,
                    entity.Content
                });
    }

    public int Delete(DocumentContentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Deleting DocumentContent with id {DocumentId} from database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        return con.Execute("delete from document_contents where documents_id=@DocumentId", new { entity.DocumentId });
    }

    public int Update(DocumentContentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Updating DocumentContent with id {DocumentId} in database", entity.DocumentId);
        _logger.LogTrace("DocumentContent: {DocumentContent}", entity);
        return con.Execute(
                "update document_contents set content = @Content where id = @DocumentId",
                new
                {
                    entity.Content,
                    entity.DocumentId
                });
    }
}

using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentRepository : IDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentRepository> _logger;

    public NpgDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentRepository> logger) {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public DocumentModel Get(int id)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<DocumentModel>("select * from documents "+
                                              "where id=@Id", new {id});
    }

    public IEnumerable<DocumentModel> GetAll()
    {
        _logger.LogDebug("Retrieving all Documents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentModel>($"select * from documents");
    }

    public int Add(DocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
            "insert into documents(id, title, author, date, summary, path, total_words, sources_id)" +
                    " values (@Id, @Title, @Author, @Date, @Summary, @Path, @TotalWords, @Source_Id)",
                new
                {
                    entity.Id,
                    entity.Title,
                    entity.Author,
                    entity.Date,
                    entity.Summary,
                    entity.Path,
                    entity.TotalWords,
                    entity.Source_Id
                });
    }

    public int Delete(DocumentModel entity)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from documents "+
                           "where id=@Id", new { entity.Id });
    }

    public int Update(DocumentModel entity)
    {
        _logger.LogDebug("Updating Document with id {Id} in database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
             "update documents set title = @Title, author = @Author, date = @Date, summary = @Summary, "+
             "path = @Path, total_words = @TotalWords, sources_id = @Source_Id "+
             "where id = @Id",
                new
                {
                    entity.Title,
                    entity.Author,
                    entity.Date,
                    entity.Summary,
                    entity.Path,
                    entity.TotalWords,
                    entity.Source_Id,
                    entity.Id
                });
    }
}

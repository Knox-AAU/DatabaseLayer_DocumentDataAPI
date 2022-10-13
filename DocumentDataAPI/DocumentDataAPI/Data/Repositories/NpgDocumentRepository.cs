using System.Data;
using System.Text;
using Dapper;
using DocumentDataAPI.Controllers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentRepository : IDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentRepository> _logger;

    public NpgDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public DocumentModel? Get(long id)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentModel>("select * from documents where id=@Id", new { id })
            .SingleOrDefault();
    }

    public IEnumerable<DocumentModel> GetAll()
    {
        _logger.LogDebug("Retrieving all Documents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentModel>($"select * from documents");
    }

    public IEnumerable<DocumentModel> GetAll(SearchParameters parameters)
    {
        _logger.LogDebug("Retrieving all Documents that the given search parameters from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        StringBuilder query = new ("select * from documents");
        if (parameters.Parameters.Any())
        {
            QueryParameter firstParam = parameters.Parameters.First();
            query.Append($" where {firstParam.Key} {firstParam.ComparisonOperator} @{firstParam.Key}");
            foreach (QueryParameter param in parameters.Parameters.Skip(1))
            {
                query.Append($" and {param.Key} {param.ComparisonOperator} @{param.Key}");
            }
        }

        DynamicParameters args = new();
        foreach (QueryParameter param in parameters.Parameters)
        {
            args.Add(param.Key, param.Value);
        }
        
        return con.Query<DocumentModel>(query.ToString(), args);
    }

    public int Add(DocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
            "insert into documents(id, title, author, date, summary, path, total_words, sources_id)" +
            " values (@Id, @Title, @Author, @Date, @Summary, @Path, @TotalWords, @SourceId)",
            new
            {
                entity.Id,
                entity.Title,
                entity.Author,
                entity.Date,
                entity.Summary,
                entity.Path,
                entity.TotalWords,
                entity.SourceId
            });
    }

    public int Delete(DocumentModel entity)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from documents " +
                           "where id=@Id", new { entity.Id });
    }

    public int Update(DocumentModel entity)
    {
        _logger.LogDebug("Updating Document with id {Id} in database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
            "update documents set title = @Title, author = @Author, date = @Date, summary = @Summary, " +
            "path = @Path, total_words = @TotalWords, sources_id = @SourceId " +
            "where id = @Id",
            new
            {
                entity.Title,
                entity.Author,
                entity.Date,
                entity.Summary,
                entity.Path,
                entity.TotalWords,
                entity.SourceId,
                entity.Id
            });
    }
}

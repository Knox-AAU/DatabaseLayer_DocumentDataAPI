using System;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;

using static Dapper.SqlMapper;

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

    public async Task<DocumentModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentModel>("select * from documents where id=@Id", new { id });
    }

    public async Task<IEnumerable<DocumentModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Documents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>($"select * from documents");
    }

    public async Task<int> Add(DocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
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

    public async Task<int> Delete(DocumentModel entity)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync("delete from documents " +
                                      "where id=@Id", new { entity.Id });
    }

    public async Task<int> Update(DocumentModel entity)
    {
        _logger.LogDebug("Updating Document with id {Id} in database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
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

    public async Task<IEnumerable<DocumentModel>> GetByAuthor(string author)
    {
        _logger.LogDebug("Retrieving Documents by {author} from database", author);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>($"select * from documents where author = @Author",
                        new
                        {
                            author
                        });
    }

    public async Task<IEnumerable<DocumentModel>> GetByDate(DateTime dateTime)
    {
        _logger.LogDebug("Retrieving Documents by {dateTime} from database", dateTime);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>($"select * from documents where date::date = @DateTime::date",
                        new
                        {
                            dateTime
                        });
    }

    public async Task<IEnumerable<DocumentModel>> GetBySource(int sourceId)
    {
        _logger.LogDebug("Retrieving Documents by {sourceId} from database", sourceId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>($"select * from documents where sources_id = @SourceId",
                        new
                        {
                            sourceId
                        });
    }

    public async Task<int> GetTotalDocumentCount()
    {
        _logger.LogDebug("Retrieving Document count from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<int>("select count(id) from documents");
    }
}

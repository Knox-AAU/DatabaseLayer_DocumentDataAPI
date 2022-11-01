using System.Data;
using System.Text;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDocumentRepository : IDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<DocumentModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentModel>($"select * from documents where {DocumentMap.Id} = @Id", new { id });
    }

    public async Task<int> Delete(long id)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from documents where {DocumentMap.Id} = @Id",
            new { id });
    }

    public async Task<IEnumerable<DocumentModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Documents from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>("select * from documents");
    }

    public async Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters)
    {
        _logger.LogDebug("Retrieving all Documents that the given search parameters from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        StringBuilder query = new("select * from documents");
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

        return await con.QueryAsync<DocumentModel>(query.ToString(), args);
    }

    public async Task<long> Add(DocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"insert into documents ({DocumentMap.Id}, {DocumentMap.Title}, {DocumentMap.Author}, {DocumentMap.Date}, {DocumentMap.Summary}, {DocumentMap.Path}, {DocumentMap.TotalWords}, {DocumentMap.SourceId}, {DocumentMap.CategoryId}, {DocumentMap.Publication}, {DocumentMap.UniqueWords})" +
            "values (@Id, @Title, @Author, @Date, @Summary, @Path, @TotalWords, @SourceId, @CategoryId, @Publication, @UniqueWords)",
            new
            {
                entity.Id,
                entity.Title,
                entity.Author,
                entity.Date,
                entity.Summary,
                entity.Path,
                entity.TotalWords,
                entity.SourceId,
                entity.CategoryId,
                entity.Publication,
                entity.UniqueWords
            });
    }

    public async Task<int> AddBatch(List<DocumentModel> models)
    {
        int rowsAffected = 0;
        _logger.LogDebug("Adding {count} documents to database", models.Count);
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (DocumentModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                rowsAffected += await transaction.ExecuteAsync(
                    $"insert into documents ({DocumentMap.Id}, {DocumentMap.SourceId}, {DocumentMap.CategoryId}, {DocumentMap.Publication}, {DocumentMap.Title}, {DocumentMap.Path}, {DocumentMap.Summary}, {DocumentMap.Date}, {DocumentMap.Author}, {DocumentMap.TotalWords}, {DocumentMap.UniqueWords}) " +
                     "values " + parameterString, parameters);
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
            _logger.LogError(e, "Failed to insert documents");
            throw;
        }
        return rowsAffected;
    }

    public async Task<int> Delete(DocumentModel entity)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from documents where {DocumentMap.Id} = @Id",
            new { entity.Id });
    }

    public async Task<int> Update(DocumentModel entity)
    {
        _logger.LogDebug("Updating Document with id {Id} in database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"update documents set {DocumentMap.Title} = @Title, {DocumentMap.Author} = @Author, {DocumentMap.Date} = @Date, {DocumentMap.Summary} = @Summary, " +
            $"{DocumentMap.Path} = @Path, {DocumentMap.TotalWords} = @TotalWords, {DocumentMap.SourceId} = @SourceId, {DocumentMap.CategoryId} = @CategoryId, " +
            $"{DocumentMap.Publication} = @Publication, {DocumentMap.UniqueWords} = @UniqueWords " +
            $"where {DocumentMap.Id} = @Id",
                        new
                        {
                            entity.Title,
                            entity.Author,
                            entity.Date,
                            entity.Summary,
                            entity.Path,
                            entity.TotalWords,
                            entity.SourceId,
                            entity.Id,
                            entity.CategoryId,
                            entity.Publication,
                            entity.UniqueWords
                        });
    }

    public async Task<int> GetTotalDocumentCount()
    {
        _logger.LogDebug("Retrieving Document count from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<int>($"select count({DocumentMap.Id}) from documents");
    }
}

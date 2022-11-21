using System.Data;
using System.Text;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
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

    public async Task<DocumentModel?> Get(long documentId)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", documentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentModel>($"select * from documents where {DocumentMap.Id} = @Id", new { id = documentId });
    }

    public async Task<int> Delete(long documentId)
    {
        _logger.LogDebug("Deleting Document with id {Id} from database", documentId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from documents where {DocumentMap.Id} = @Id",
            new { id = documentId });
    }

    public async Task<IEnumerable<DocumentModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Documents from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from documents", limit, offset, DocumentMap.Id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>(sql);
    }

    public async Task<IEnumerable<DocumentModel>> GetAll(DocumentSearchParameters parameters, int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Documents that the given search parameters from database");
        StringBuilder query = new("select * from documents");
        DynamicParameters args = new();
        if (parameters.Parameters.Any())
        {
            QueryParameter firstParam = parameters.Parameters.First();
            args.Add(firstParam.Key, firstParam.Value);
            query.Append(" where " + _sqlHelper.GetParameterString(firstParam));
            foreach (QueryParameter param in parameters.Parameters.Skip(1))
            {
                query.Append(" and " + _sqlHelper.GetParameterString(param));
                args.Add(param.Key, param.Value);
            }
        }
        string sql = _sqlHelper.GetPaginatedQuery(query.ToString(), limit, offset, DocumentMap.Id);

        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DocumentModel>(sql, args);
    }

    public async Task<long> Add(DocumentModel entity)
    {
        _logger.LogDebug("Adding Document with id {Id} to database", entity.Id);
        _logger.LogTrace("Document: {Document}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<long>(
            $"insert into documents ({DocumentMap.Title}, {DocumentMap.Author}, {DocumentMap.Date}, {DocumentMap.Summary}, {DocumentMap.Path}, {DocumentMap.TotalWords}, {DocumentMap.SourceId}, {DocumentMap.CategoryId}, {DocumentMap.Publication}, {DocumentMap.UniqueWords})" +
            $"values (@Title, @Author, @Date, @Summary, @Path, @TotalWords, @SourceId, @CategoryId, @Publication, @UniqueWords) returning {DocumentMap.Id}",
            new
            {
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

    public async Task<IEnumerable<long>> AddBatch(List<DocumentModel> models)
    {
        IEnumerable<long> allInsertedIds = new List<long>();
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
                IEnumerable<long> insertedIds = await transaction.QueryAsync<long>(
                    $"insert into documents ({DocumentMap.SourceId}, {DocumentMap.CategoryId}, {DocumentMap.Publication}, {DocumentMap.Title}, {DocumentMap.Path}, {DocumentMap.Summary}, {DocumentMap.Date}, {DocumentMap.Author}, {DocumentMap.TotalWords}, {DocumentMap.UniqueWords}) " +
                    $"values {parameterString} returning {DocumentMap.Id}",
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

    public async Task<IEnumerable<string>> GetAuthors()
    {
        _logger.LogDebug("Retrieving all authors from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<string>($"select distinct {DocumentMap.Author} from documents");
    }
}

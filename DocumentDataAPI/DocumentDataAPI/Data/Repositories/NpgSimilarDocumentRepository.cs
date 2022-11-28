using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using System.Data;

namespace DocumentDataAPI.Data.Repositories;

class NpgSimilarDocumentRepository : ISimilarDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSimilarDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;
    public NpgSimilarDocumentRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSimilarDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<long> Add(SimilarDocumentModel entity)
    {
        _logger.LogDebug("Adding Source with mainDocumentId: {mainDocumentId} and" +
            " similarDocumentId: {similarDocumentId} to database", entity.MainDocumentId, entity.SimilarDocumentId);
        _logger.LogTrace("similarDocument: {similarDocument}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<long>($"insert into similar_documents({SimilarDocumentMap.MainDocumentId}, {SimilarDocumentMap.SimilarDocumentId}) values (@MainDocumentId, @SimilarDocumentId) returning {SimilarDocumentMap.MainDocumentId}",
            new { entity.MainDocumentId, entity.SimilarDocumentId });
    }

    public Task<IEnumerable<long>> AddBatch(List<SimilarDocumentModel> models)
    {
        throw new NotImplementedException();
    }

    public Task<int> Delete(long mainDocumentId, long similarDocumentId)
    {
        throw new NotImplementedException();
    }

    public Task<SimilarDocumentModel?> Get(long mainDocumentId, long similarDocumentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SimilarDocumentModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Similar Documents from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from similar_documents", limit, offset);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SimilarDocumentModel>(sql);
    }

    public Task<int> Update(SimilarDocumentModel entity)
    {
        throw new NotImplementedException();
    }
}
using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSourceRepository : ISourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSourceRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgSourceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSourceRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<SourceModel?> Get(long sourceId)
    {
        _logger.LogDebug("Retrieving Source with id {id} from database", sourceId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<SourceModel>($"select * from sources where {SourceMap.Id} = @Id",
            new { id = sourceId });
    }

    public async Task<int> Delete(long sourceId)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", sourceId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from sources where {SourceMap.Id} = @Id",
            new { id = sourceId });
    }

    public async Task<IEnumerable<SourceModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all Sources from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from sources", limit, offset, SourceMap.Id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SourceModel>(sql);
    }

    public async Task<long> Add(SourceModel entity)
    {
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<long>($"insert into sources({SourceMap.Name}) values (@Name) returning {SourceMap.Id}",
            new { entity.Name });
    }

    public async Task<int> Update(SourceModel entity)
    {
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"update sources set {SourceMap.Name} = @Name where {SourceMap.Id} = @Id",
            new { entity.Name, entity.Id });
    }

    public async Task<long> GetCountFromId(long id)
    {
        _logger.LogDebug("Retrieving Document count with sources_id {id}", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleOrDefaultAsync<int>(
            $"select COUNT(*) document_count from documents where {DocumentMap.SourceId} = @Id",
            new { id });
    }

    public async Task<IEnumerable<SourceModel>> GetByName(string name)
    {
        _logger.LogDebug("Retrieving sources with name: {name}", name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<SourceModel>($"select * from sources where {SourceMap.Name} = @Name",
            new { name });
    }
}

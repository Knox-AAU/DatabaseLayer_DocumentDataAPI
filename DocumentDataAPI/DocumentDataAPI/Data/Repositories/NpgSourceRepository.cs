using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSourceRepository : ISourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSourceRepository> _logger;

    public NpgSourceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSourceRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<SourceModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving Source with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<SourceModel>($"select * from sources where {SourceMap.Id} = @Id",
            new { id });
    }

    public async Task<int> Delete(long id)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from sources where {SourceMap.Id} = @Id",
            new { id });
    }

    public async Task<IEnumerable<SourceModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Sources from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SourceModel>("select * from sources");
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

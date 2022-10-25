using System.Data;
using Dapper;
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
        return await con.QueryFirstOrDefaultAsync<SourceModel>("select * from data_sources where id=@Id", new { id });
    }

    public async Task<IEnumerable<SourceModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Sources from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<SourceModel>($"select * from data_sources");
    }

    public async Task<int> Add(SourceModel entity)
    {
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "insert into data_sources(name) " +
            "values (@Name)", new { entity.Name });
    }

    public async Task<int> Delete(SourceModel entity)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "delete from data_sources " +
            "where id=@Id", new { entity.Id });
    }

    public async Task<int> Update(SourceModel entity)
    {
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            "update data_sources set name = @Name " +
            "where id = @Id", new { entity.Name, entity.Id });
    }

    public async Task<long> GetCountFromId(long id)
    {
        _logger.LogDebug("Retrieving Document count with sources_id {id}", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleOrDefaultAsync<int>(
            "select COUNT(*) as document_count from documents " +
            "where sources_id=@Id", new { id });
    }

    public async Task<IEnumerable<SourceModel>> GetByName(string name)
    {
        _logger.LogDebug("Retrieving sources with name: {name}", name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<SourceModel>("select * from data_sources where name = @Name", new { name });
    }
}

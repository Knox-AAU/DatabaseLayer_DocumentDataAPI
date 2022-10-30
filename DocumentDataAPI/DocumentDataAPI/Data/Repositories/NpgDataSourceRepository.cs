using System.Data;
using Dapper;
using Dapper.FluentMap.Mapping;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgDataSourceRepository : IDataSourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDataSourceRepository> _logger;

    public NpgDataSourceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDataSourceRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<DataSourceModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving Source with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DataSourceModel>("select * from data_sources where id=@Id",
            new { id });
    }

    public async Task<IEnumerable<DataSourceModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Sources from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DataSourceModel>("select * from data_sources");
    }

    public async Task<long> Add(DataSourceModel entity)
    {
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleAsync<long>("insert into data_sources(name) values (@Name) returning id",
            new { entity.Name });
    }

    public async Task<int> Delete(DataSourceModel entity)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync("delete from data_sources where id=@Id",
            new { entity.Id });
    }

    public async Task<int> Update(DataSourceModel entity)
    {
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync("update data_sources set name = @Name where id = @Id",
            new { entity.Name, entity.Id });
    }

    public async Task<long> GetCountFromId(long id)
    {
        _logger.LogDebug("Retrieving Document count with sources_id {id}", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleOrDefaultAsync<int>("select COUNT(*) document_count from documents where data_sources_id=@Id",
            new { id });
    }

    public async Task<IEnumerable<DataSourceModel>> GetByName(string name)
    {
        _logger.LogDebug("Retrieving sources with name: {name}", name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<DataSourceModel>("select * from data_sources where name = @Name",
            new { name });
    }
}

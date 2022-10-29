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

    // Column name helpers
    private readonly EntityMap<DataSourceModel> _map = new DataSourceMap();
    private string IdColumn => _map.GetMappedColumnName(nameof(DataSourceModel.Id));
    private string NameColumn => _map.GetMappedColumnName(nameof(DataSourceModel.Name));
    private string DataSourceIdColumn => new DocumentMap().GetMappedColumnName(nameof(DocumentModel.DataSourceId));

    public NpgDataSourceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgDataSourceRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<DataSourceModel?> Get(long id)
    {
        _logger.LogDebug("Retrieving Source with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DataSourceModel>($"select * from data_sources where {IdColumn}=@Id",
            new { id });
    }

    public async Task<IEnumerable<DataSourceModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all Sources from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<DataSourceModel>("select * from data_sources");
    }

    public async Task<int> Add(DataSourceModel entity)
    {
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"insert into data_sources({NameColumn}) values (@Name)",
            new { entity.Name });
    }

    public async Task<int> Delete(DataSourceModel entity)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"delete from data_sources where {IdColumn}=@Id",
            new { entity.Id });
    }

    public async Task<int> Update(DataSourceModel entity)
    {
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync($"update data_sources set {NameColumn} = @Name where {IdColumn} = @Id",
            new { entity.Name, entity.Id });
    }

    public async Task<long> GetCountFromId(long id)
    {
        _logger.LogDebug("Retrieving Document count with sources_id {id}", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QuerySingleOrDefaultAsync<int>($"select COUNT(*) document_count from documents where {DataSourceIdColumn}=@Id",
            new { id });
    }

    public async Task<IEnumerable<DataSourceModel>> GetByName(string name)
    {
        _logger.LogDebug("Retrieving sources with name: {name}", name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<DataSourceModel>($"select * from data_sources where {NameColumn} = @Name",
            new { name });
    }
}

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

    public SourceModel? Get(long id)
    {
        _logger.LogDebug("Retrieving Source with id {id} from database", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<SourceModel>("select * from sources where id=@Id", new { id })
            .SingleOrDefault();
    }

    public IEnumerable<SourceModel> GetAll()
    {
        _logger.LogDebug("Retrieving all Sources from database");
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<SourceModel>($"select * from sources");
    }

    public int Add(SourceModel entity)
    {
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("insert into sources(name) " +
                           "values (@Name)", new { entity.Name });
    }

    public int Delete(SourceModel entity)
    {
        _logger.LogDebug("Deleting Source with id {Id} from database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from sources " +
                           "where id=@Id", new { entity.Id });
    }

    public int Update(SourceModel entity)
    {
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("update sources set name = @Name " +
                           "where id = @Id", new { entity.Name, entity.Id });
    }

    public long GetCountFromId(long id)
    {
        _logger.LogDebug("Retrieving Document count with sources_id {id}", id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<int>("select COUNT(*) as document_count from documents " +
                                    "where sources_id=@Id", new { id });
    }

    public IEnumerable<SourceModel> GetByName(string name)
    {
        _logger.LogDebug("Retrieving sources with name: {name}", name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return connection.Query<SourceModel>("select * from sources where name = @Name", new { name });
    }
}

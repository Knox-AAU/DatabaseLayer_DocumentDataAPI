using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSourceRepository : ISourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSourceRepository> _logger;

    public NpgSourceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSourceRepository> logger) {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public SourceModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Retrieving Source with id {id} from database", id);
        return con.QuerySingle<SourceModel>("select * from sources where id=@Id", new { id });
    }

    public IEnumerable<SourceModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Retrieving all Sources from database");
        return con.Query<SourceModel>($"select * from sources");
    }

    public int Add(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Adding Source with id {Id} to database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        return con.Execute("insert into sources(name) values (@Name)", new { entity.Name });
    }

    public int Delete(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Deleting Source with id {Id} from database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        return con.Execute("delete from sources where id=@Id", new { entity.Id });
    }

    public int Update(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        _logger.LogDebug("Updating Source with id {Id} in database", entity.Id);
        _logger.LogTrace("Source: {Source}", entity);
        return con.Execute("update sources set name = @Name where id = @Id", new { entity.Name, entity.Id });
    }
}

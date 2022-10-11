using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSourceRepository : ISourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NpgSourceRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public SourceModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<SourceModel>("select * from sources where id=@Id", new { id });
    }

    public IEnumerable<SourceModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<SourceModel>($"select * from sources");
    }

    public int Add(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("insert into sources(name) values (@Name)", new { entity.Name });
    }

    public int Delete(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from sources where id=@Id", new { entity.Id });
    }

    public int Update(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("update sources set name = @Name where id = @Id", new { entity.Name, entity.Id });
    }
}

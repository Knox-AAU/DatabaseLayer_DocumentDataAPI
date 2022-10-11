using System.Data;
using Dapper;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SourceRepository(IDbConnectionFactory connectionFactory)
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

    public void Delete(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Execute("delete from sources where id=@Id", new { entity.Id });
    }

    public void Update(SourceModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Execute("update sources set name = @Name where id = @Id", new { entity.Name, entity.Id });
    }

    public int GetCountFromId(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<int>("select COUNT(*) as document_count from documents where sources_id=@Id",
            new { id });
    }
}

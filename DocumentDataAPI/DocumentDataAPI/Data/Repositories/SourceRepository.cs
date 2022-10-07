using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class SourceRepository : IRepository<SourceModel>, ISourceRepository
{
    private readonly DatabaseOptions _options;
    public SourceRepository(IConfiguration config)
    {
        _options = config.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();
    }

    public SourceModel Get(int id)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        SourceModel res;
        using (con)
        {
            res = con.QuerySingle<SourceModel>("select * from sources where id=@Id",
                new
                {
                    id
                });
        }
        return res;
    }

    public IEnumerable<SourceModel> GetAll()
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        List<SourceModel> res;
        using (con)
        {
            res = con.Query<SourceModel>($"select * from sources").ToList();
        }
        return res;
    }

    public void Add(SourceModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            con.Execute(
            "insert into sources(name)" +
                    " values (@@Name)",
                new
                {
                    entity.Name
                });
        }
    }

    public void Delete(SourceModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            con.Execute("delete from sources where id=@Id", new { entity.Id });
        }
    }

    public void Update(SourceModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            con.Execute(
                "update sources set name = @Name where id = @Id",
                new
                {
                    entity.Name,
                    entity.Id
                });
        }
    }

    public int GetCountFromId(int id)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        int res;
        using (con)
        {
            res = con.QuerySingle<int>("select COUNT(*) as document_count from documents where sources_id=@Id",
                new
                {
                    id
                });
        }
        return res;
    }
}

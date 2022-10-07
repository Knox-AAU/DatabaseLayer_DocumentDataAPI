using System.Collections.Generic;
using System.Data;
using Dapper;

namespace WordCount.Data.Repositories;

public class SourceRepository : IRepository<SourceModel>
{
    private IApplicationProvider _applicationProvider;

    public SourceRepository(IApplicationProvider applicationProvider)
    {
        _applicationProvider = applicationProvider;
    }

    public SourceModel Get(int id)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        SourceModel res = new();
        using (con)
        {
            res = con.Query<SourceModel>("select * from sources where id=@Id",
                new
                {
                    id
                });   
        }
        return res;
    }

    public IEnumerable<SourceModel> GetAll()
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        IEnumerable<SourceModel> res = new();
        using (con)
        {
            res = con.Query<SourceModel>($"select * from sources");
        }
        return res;
    }

    public void Add(SourceModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
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
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con) 
        {
            con.Execute("delete from sources where id=@Id", new { entity.Id });
        }
    }

    public void Update(SourceModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute(
                "update sources set name = @Name where id = @Id",
                new
                {
                    entity.Name, entity.Id
                });
        }
    }
}
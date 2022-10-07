using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class DocumentContentRepository : IRepository<DocumentContentModel>
{
    DatabaseOptions options;
    public DocumentContentRepository(IConfiguration config)
    {
        options = config.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();
    }

    public DocumentContentModel Get(int id)
    {
        IDbConnection con = new NpgsqlConnection(options.ConnectionString);
        DocumentContentModel res = new();
        using (con)
        {
            res = con.QuerySingle<DocumentContentModel>("select * from document_contents where documents_id=@Id",
                new
                {
                    id
                });
        }
        return res;
    }

    public IEnumerable<DocumentContentModel> GetAll()
    {
        IDbConnection con = new NpgsqlConnection(options.ConnectionString);
        List<DocumentContentModel> res = new();
        using (con)
        {
            res = con.Query<DocumentContentModel>($"select * from document_contents").ToList();
        }
        return res;
    }

    public void Add(DocumentContentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(options.ConnectionString);
        using (con)
        {
            con.Execute(
            "insert into document_contents(documents_id, content)" +
                    " values (@DocumentId, @Content)",
                new
                {
                    entity.DocumentId,
                    entity.Content
                });
        }
    }

    public void Delete(DocumentContentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(options.ConnectionString);
        using (con)
        {
            con.Execute("delete from document_contents where documents_id=@DocumentId", new { entity.DocumentId });
        }
    }

    public void Update(DocumentContentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(options.ConnectionString);
        using (con)
        {
            con.Execute(
                "update document_contents set content = @Content where id = @DocumentId",
                new
                {
                    entity.Content,
                    entity.DocumentId
                });
        }
    }
}
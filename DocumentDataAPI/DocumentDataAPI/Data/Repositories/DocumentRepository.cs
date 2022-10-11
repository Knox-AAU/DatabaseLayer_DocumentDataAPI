using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class DocumentRepository : IRepository<DocumentModel>
{
    private readonly DatabaseOptions _options;

    public DocumentRepository(IConfiguration config)
    {
        _options = config.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();
    }

    public DocumentModel Get(int id)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        DocumentModel res = new();
        using (con)
        {
            res = con.QuerySingle<DocumentModel>("select * from documents where id=@Id",
                new
                {
                    id
                });
        }

        return res;
    }

    public IEnumerable<DocumentModel> GetAll()
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        List<DocumentModel> res = new();
        using (con)
        {
            res = con.Query<DocumentModel>($"select * from documents").ToList();
        }

        return res;
    }

    public int Add(DocumentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            return con.Execute(
                "insert into documents(id, title, author, date, summary, path, total_words, sources_id)" +
                " values (@Id, @Title, @Author, @Date, @Summary, @Path, @TotalWords, @Source_Id)",
                new
                {
                    entity.Id,
                    entity.Title,
                    entity.Author,
                    entity.Date,
                    entity.Summary,
                    entity.Path,
                    entity.TotalWords,
                    entity.Source_Id
                });
        }
    }

    public int Delete(DocumentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            return con.Execute("delete from documents where id=@Id", new { entity.Id });
        }
    }

    public int Update(DocumentModel entity)
    {
        IDbConnection con = new NpgsqlConnection(_options.ConnectionString);
        using (con)
        {
            return con.Execute(
                "update documents set title = @Title, author = @Author, date = @Date, summary = @Summary, path = @Path, total_words = @TotalWords, sources_id = @Source_Id where id = @Id",
                new
                {
                    entity.Title,
                    entity.Author,
                    entity.Date,
                    entity.Summary,
                    entity.Path,
                    entity.TotalWords,
                    entity.Source_Id,
                    entity.Id
                });
        }
    }
}

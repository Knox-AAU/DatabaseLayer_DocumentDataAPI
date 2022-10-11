using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Microsoft.CodeAnalysis;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class DocumentRepository : IRepository<DocumentModel>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DocumentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public DocumentModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<DocumentModel>("select * from documents where id=@Id", new {id});
    }

    public IEnumerable<DocumentModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentModel>($"select * from documents");
    }

    public int Add(DocumentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
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

    public int Delete(DocumentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from documents where id=@Id", new { entity.Id });
    }

    public int Update(DocumentModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
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

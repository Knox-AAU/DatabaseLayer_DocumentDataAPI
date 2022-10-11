using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class DocumentContentRepository : IRepository<DocumentContentModel>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DocumentContentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public DocumentContentModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<DocumentContentModel>("select * from document_contents where documents_id=@Id",
            new {id});
    }

    public IEnumerable<DocumentContentModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<DocumentContentModel>("select * from document_contents");
    }

    public void Add(DocumentContentModel entity)
    {
        IDbConnection con = _connectionFactory.CreateConnection();
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
        IDbConnection con = _connectionFactory.CreateConnection();
        using (con)
        {
            con.Execute("delete from document_contents where documents_id=@DocumentId", new { entity.DocumentId });
        }
    }

    public void Update(DocumentContentModel entity)
    {
        IDbConnection con = _connectionFactory.CreateConnection();
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

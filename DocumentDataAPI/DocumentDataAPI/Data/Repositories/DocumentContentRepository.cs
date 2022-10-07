using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;


namespace DocumentDataAPI.Data.Repositories;

public class DocumentContentRepository : IRepository<DocumentContentModel>
{
    private IApplicationProvider _applicationProvider;

    public DocumentContentRepository(IApplicationProvider applicationProvider)
    {
        _applicationProvider = applicationProvider;
    }

    public DocumentContentModel Get(int id)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
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
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        List<DocumentContentModel> res = new();
        using (con)
        {
            res = con.Query<DocumentContentModel>($"select * from document_contents").ToList();
        }
        return res;
    }

    public void Add(DocumentContentModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
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
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute("delete from document_contents where documents_id=@DocumentId", new { entity.DocumentId });
        }
    }

    public void Update(DocumentContentModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
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
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace WordCount.Data.Repositories;

public class DocumentRepository : IRepository<DocumentModel>
{
    private IApplicationProvider _applicationProvider;

    public DocumentRepository(IApplicationProvider applicationProvider)
    {
        _applicationProvider = applicationProvider;
    }

    public DocumentModel Get(int id)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        DocumentModel res = new();
        using (con)
        {
            res = con.Query<DocumentModel>("select * from documents where id=@Id",
                new
                {
                    id
                });   
        }
        return res;
    }

    public IEnumerable<DocumentModel> GetAll()
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        IEnumerable<DocumentModel> res = new();
        using (con)
        {
            res = con.Query<DocumentModel>($"select * from documents");
        }
        return res;
    }

    public void Add(DocumentModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute(
            "insert into documents(id, title, author, date, summary, path, total_words, sources_id)" +
                    " values (@Id, @Title, @Author, @Date, @Summary, @Path, @TotalWords, @Source_Id)",
                new
                {
                    entity.Id, entity.Title, entity.Author, entity.Date, entity.Summary, entity.Path, entity.TotalWords, entity.Source_Id
                });           
        }
    }

    public void Delete(DocumentModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con) 
        {
            con.Execute("delete from documents where id=@Id", new { entity.Id });
        }
    }

    public void Update(DocumentModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute(
                "update documents set title = @Title, author = @Author, date = @Date, summary = @Summary, path = @Path, total_words = @TotalWords, sources_id = @Source_Id where id = @Id",
                new
                {
                    entity.Title, entity.Author, entity.Date, entity.Summary, entity.Path, entity.TotalWords, entity.Source_Id, entity.Id
                });
        }
    }
}
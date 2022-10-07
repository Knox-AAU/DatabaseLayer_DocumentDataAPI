using System.Collections.Generic;
using System.Data;
using Dapper;

namespace WordCount.Data.Repositories;

public class WordRatioRepository : IRepository<WordRatioModel>
{
    private IApplicationProvider _applicationProvider;

    public WordRatioRepository(IApplicationProvider applicationProvider)
    {
        _applicationProvider = applicationProvider;
    }

    public WordRatioModel Get(int id)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        WordRatioModel res = new();
        using (con)
        {
            res = con.Query<WordRatioModel>("select * from word_ratios where id=@Id",
                new
                {
                    id
                });   
        }
        return res;
    }

    public IEnumerable<WordRatioModel> GetAll()
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        IEnumerable<WordRatioModel> res = new();
        using (con)
        {
            res = con.Query<WordRatioModel>($"select * from word_ratios");
        }
        return res;
    }

    public void Add(WordRatioModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute(
            "insert into word_ratios(documents_id, word, amount, percent, rank)" +
                    " values (@DocumentId, @Word, @Amount, @Percent, @Rank)",
                new
                {
                    entity.DocumentId, entity.Word, entity.Amount, entity.Percent, entity.Rank
                });           
        }
    }

    public void Delete(WordRatioModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con) 
        {
            con.Execute("delete from word_ratios where documents_id=@DocumentId", new { entity.DocumentId });
        }
    }

    public void Update(WordRatioModel entity)
    {
        IDbConnection con = _applicationProvider.GetService<IDbConnection>();
        using (con)
        {
            con.Execute(
                "update word_ratios set word = @Word, amount = @Amount, percent = @Percent, rank = @Rank where documents_id = @DocumentId",
                new
                {
                    entity.Word, entity.Amount, entity.Percent, entity.Rank, entity.DocumentId
                });
        }
    }
}
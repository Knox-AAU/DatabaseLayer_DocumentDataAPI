using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentDataAPI.Models;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Repositories;

public class WordRatioRepository : IRepository<WordRatioModel>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public WordRatioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public WordRatioModel Get(int id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.QuerySingle<WordRatioModel>("select * from word_ratios where id=@Id",
                new
                {
                    id
                });
    }

    public IEnumerable<WordRatioModel> GetAll()
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Query<WordRatioModel>($"select * from word_ratios");
    }

    public int Add(WordRatioModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
            "insert into word_ratios(documents_id, word, amount, percent, rank)" +
                    " values (@DocumentId, @Word, @Amount, @Percent, @Rank)",
                new
                {
                    entity.DocumentId,
                    entity.Word,
                    entity.Amount,
                    entity.Percent,
                    entity.Rank
                });
    }

    public int Delete(WordRatioModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute("delete from word_ratios where documents_id=@DocumentId", new { entity.DocumentId });
    }

    public int Update(WordRatioModel entity)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return con.Execute(
                "update word_ratios set word = @Word, amount = @Amount, percent = @Percent, rank = @Rank where documents_id = @DocumentId",
                new
                {
                    entity.Word,
                    entity.Amount,
                    entity.Percent,
                    entity.Rank,
                    entity.DocumentId
                });
    }
}

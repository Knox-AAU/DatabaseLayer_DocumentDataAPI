using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Options;

namespace DocumentDataAPI.Data.Repositories;

public class NpgWordRelevanceRepository : IWordRelevanceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRelevanceRepository> _logger;

    public NpgWordRelevanceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgWordRelevanceRepository> logger)
    {
        _connectionFactory = connectionFactory.WithSchema(DatabaseOptions.Schema.DocumentData);
        _logger = logger;
    }

    public async Task<int> UpdateWordRelevances()
    {
        _logger.LogInformation("Updating all TF-IDF scores in database");
        // Create temporary table with the IDF value of each distinct word
        // Then set TF-IDF of each word_ratio to the TF of the word (Percent) times the IDF value for the word.
        const string updateSql = $@"
create temp table temp_idf_values as 
    select q1.word, ln(q2.total_documents::decimal / q1.documents_for_word) idf
    from (
        select {WordRatioMap.Word}, count(distinct {WordRatioMap.DocumentId}) documents_for_word
        from word_ratios
        group by {WordRatioMap.Word}
    )q1
    join (
        select count(1) total_documents
        from documents
    )q2 on 1=1;

update word_ratios wr
    set {WordRatioMap.TfIdf} = {WordRatioMap.Percent} * temp.idf
    from temp_idf_values temp
    where wr.{WordRatioMap.Word} = temp.word;
    ";

        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(updateSql, commandTimeout: 0);
    }
}

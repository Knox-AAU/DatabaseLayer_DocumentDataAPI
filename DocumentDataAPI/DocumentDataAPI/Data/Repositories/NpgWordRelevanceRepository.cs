using System.Data;
using Dapper;
using DocumentDataAPI.Data.Repositories.Helpers;

namespace DocumentDataAPI.Data.Repositories
{
    public class NpgWordRelevanceRepository : IWordRelevanceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<NpgWordRelevanceRepository> _logger;
        private readonly ISqlHelper _sqlHelper;

        public NpgWordRelevanceRepository(IDbConnectionFactory connectionFactory, ILogger<NpgWordRelevanceRepository> logger,
            ISqlHelper sqlHelper)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _sqlHelper = sqlHelper;
        }

        public async Task<int> UpdateWordRelevances()
        {
            _logger.LogDebug("Updating all TF-IDF scores in database");
            // Cast a value to avoid integer division (default behavior in postgres)
            const string script = @"update document_data.word_ratios w1
                            set tf_idf = percent * ln(
                                (select cast(count(1) as decimal) from document_data.documents)
                                /
                                (select count(1) as docWithWordCount from document_data.documents
                                inner join document_data.word_ratios w2
                                on w2.documents_id = documents.id
                                and w2.word = w1.word)
                            )
                            where 1 = 1;";
            using IDbConnection con = _connectionFactory.CreateConnection();

            return await con.ExecuteAsync(script, commandTimeout: 0);
        }
    }
}

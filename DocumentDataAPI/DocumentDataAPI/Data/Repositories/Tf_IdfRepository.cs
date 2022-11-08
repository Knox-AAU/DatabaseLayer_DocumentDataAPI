using System.Data;
using Dapper;
using DocumentDataAPI.Data.Repositories.Helpers;

namespace DocumentDataAPI.Data.Repositories
{
    public class NpgTfIdfRepository : ITfIdfRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<NpgTfIdfRepository> _logger;
        private readonly ISqlHelper _sqlHelper;

        public NpgTfIdfRepository(IDbConnectionFactory connectionFactory, ILogger<NpgTfIdfRepository> logger, ISqlHelper sqlHelper)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _sqlHelper = sqlHelper;
        }

        public async Task<int> UpdateTfIdfs()
        {
            _logger.LogDebug("Updating all TF-IDF scores in database");
            string script = "update document_data.word_ratios w1" +
                            "set tfidf = percent * ln(" +
                                "(select count(1) from document_data.documents)" +
                                "/" +
                                "(select count(1) as docWithWordCount from document_data.documents" +
                                "inner join document_data.word_ratios w2" +
                                "on w2.documents_id = documents.id" +
                                "and w2.word = w1.word)" +
                            ")" +
                            "where 1 = 1;";
            using IDbConnection con = _connectionFactory.CreateConnection();

            return await con.ExecuteAsync(script);

        }
    }
}
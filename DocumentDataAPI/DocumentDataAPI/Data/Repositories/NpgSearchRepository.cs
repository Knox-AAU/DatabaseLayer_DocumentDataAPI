using System.Data;
using System.Text;
using Dapper;
using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgSearchRepository : ISearchRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSearchRepository> _logger;
    private readonly IRelevanceFunction _relevanceFunction;

    public NpgSearchRepository(IDbConnectionFactory connectionFactory, ILogger<NpgSearchRepository> logger, IRelevanceFunction relevanceFunction)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _relevanceFunction = relevanceFunction;
    }

    public async Task<IEnumerable<SearchResponseModel>> Get(List<string> processedWords, DocumentSearchParameters parameters)
    {
        _logger.LogDebug("Retrieving most relevant documents from database, given the search query {searchWords}", parameters);
        using IDbConnection con = _connectionFactory.CreateConnection();
        DynamicParameters args = new();
        args.Add("words", processedWords);
        StringBuilder query = new($"select distinct d.* from documents d, word_ratios w where w.{WordRatioMap.Word} = any(@words) and w.{WordRatioMap.DocumentId} = d.{DocumentMap.Id}");
        if (parameters.Parameters.Any())
        {
            foreach (QueryParameter param in parameters.Parameters)
            {
                query.Append($" and {param.Key} {param.ComparisonOperator} @{param.Key}");
                args.Add(param.Key, param.Value);
            }
        }

        IEnumerable<DocumentModel> documents = await con.QueryAsync<DocumentModel>(query.ToString(), args);
        List<SearchResponseModel> searchResponses = new();
        foreach (DocumentModel document in documents)
        {
            IEnumerable<WordRatioModel> documentWordRatios = await GetWordRatiosByDocumentId(document.Id);
            double documentRelevance = _relevanceFunction.CalculateRelevance(documentWordRatios, processedWords);
            searchResponses.Add(new SearchResponseModel(document, documentRelevance));
        }

        return searchResponses.OrderByDescending(s => s.Relevance).ThenBy(s => s.DocumentModel.Id);
    }

    private async Task<IEnumerable<WordRatioModel>> GetWordRatiosByDocumentId(long id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>($"select * from word_ratios where {WordRatioMap.DocumentId} = @DocumentId",
            new { DocumentId = id });
    }
}

using System.Collections.Concurrent;
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

        _logger.LogDebug("Retrieving most relevant documents from database, given the search query {searchWords}", parameters);
        IEnumerable<DocumentModel> documents;
        IDbConnection con = _connectionFactory.CreateConnection();
        using (con)
        {
            documents = await con.QueryAsync<DocumentModel>(query.ToString(), args);
        }

        ConcurrentBag<SearchResponseModel> searchResponses = new();
        await Parallel.ForEachAsync(documents, async (model, token) =>
        {
            IEnumerable<WordRatioModel> documentWordRatios = await GetWordRatiosByDocumentId(model.Id);
            double documentRelevance = _relevanceFunction.CalculateRelevance(documentWordRatios, processedWords);
            searchResponses.Add(new SearchResponseModel(model, documentRelevance));
        });

        return searchResponses.OrderByDescending(s => s.Relevance);
    }

    private async Task<IEnumerable<WordRatioModel>> GetWordRatiosByDocumentId(long id)
    {
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<WordRatioModel>($"select * from word_ratios where {WordRatioMap.DocumentId} = @DocumentId",
            new { DocumentId = id });
    }
}

using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgSearchRepositoryIntegrationTests
{
    private readonly NpgSearchRepository _repository;

    public NpgSearchRepositoryIntegrationTests()
    {
        NpgDbConnectionFactory connectionFactory = new(TestHelper.DatabaseOptions.ConnectionString);
        ILogger<NpgSearchRepository> logger = new Logger<NpgSearchRepository>(new NullLoggerFactory());
        IRelevanceFunction relevanceFunction = new CosineSimilarityCalculator();
        _repository = new NpgSearchRepository(connectionFactory, logger, relevanceFunction, new DapperSqlHelper(TestHelper.Configuration));
    }

    [Fact]
    public async Task Get_ReturnsAllRelevantDocuments()
    {
        // Arrange
        List<string> query = new()
        {
            "af"
        };
        List<long> expected = new()
        {
            1, 2, 3, 5
        }; // The word "af" exists in documents with ids 1, 2, 3 and 5

        // Act
        IEnumerable<SearchResponseModel> result = await _repository.Get(query, new DocumentSearchParameters());

        // Assert
        result.Should().OnlyContain(d => expected.Contains(d.DocumentModel.Id), "because that is the exhausted list of documents with the search query")
            .And.OnlyHaveUniqueItems("because we are not interested in documents being returned several times");
    }

    [Fact]
    public async Task Get_DocumentsOrderedByRelevance()
    {
        // Arrange
        List<string> query = new() { "dronningen", "døde", "og"};

        // Act
        IEnumerable<SearchResponseModel> result = await _repository.Get(query, new DocumentSearchParameters());

        // Assert
        result.Should().BeInDescendingOrder(x => x.Relevance, "because document 1 is more relevant than document 2")
            .And.SatisfyRespectively(
                x => x.DocumentModel.Id.Should().Be(1L),
                x => x.DocumentModel.Id.Should().Be(2L),
                x => x.Relevance.Should().Be(0L),
                x => x.Relevance.Should().Be(0L),
                x => x.Relevance.Should().Be(0L)
            );
        // Explanation:
        // Document 1 contains the word "døde" 3 times (TF-IDF = 4.96)
        // Document 2 contains the word "dronningen" 2 times (TF-IDF = 2.59)
        // The other documents only contain the word "og" (TF-IDF = 0, i.e. it is irrelevant to the search since all documents contain this word)
    }

    [Fact]
    public async Task Get_QueryNotFound_ReturnsEmptyList()
    {
        // Arrange
        List<string> query = new() { "videnskab" };

        // Act
        IEnumerable<SearchResponseModel> result = await _repository.Get(query, new DocumentSearchParameters());

        // Assert
        result.Should().BeEmpty("because no documents contain the term \"videnskab\"");
    }
}

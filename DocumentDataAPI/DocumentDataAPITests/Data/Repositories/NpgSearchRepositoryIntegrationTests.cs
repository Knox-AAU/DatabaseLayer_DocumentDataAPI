using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgSearchRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgSearchRepository _repository;

    public NpgSearchRepositoryIntegrationTests()
    {
        NpgDbConnectionFactory connectionFactory = new(DatabaseOptions.ConnectionString);
        ILogger<NpgSearchRepository> logger = new Logger<NpgSearchRepository>(new NullLoggerFactory());
        IRelevanceFunction relevanceFunction = new CosineSimilarityCalculator();
        _repository = new NpgSearchRepository(connectionFactory, logger, relevanceFunction, new DapperSqlHelper(Configuration));
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

    [Fact]
    public async Task GetAll_WithSpecificLimitAndOffset_ReturnsConsistentResults()
    {
        //Arrange
        List<string> query = new() { "dronningen", "døde", "og" };
        const int limit = 3;
        const int firstOffset = 0,
            nextOffset = 3;

        //Act
        IEnumerable<SearchResponseModel> firstResult = await _repository.Get(query, new DocumentSearchParameters(), limit, firstOffset);
        IEnumerable<SearchResponseModel> nextResult = await _repository.Get(query, new DocumentSearchParameters(), limit, nextOffset);

        //Assert
        firstResult.Should().SatisfyRespectively(
            x => x.DocumentModel.Id.Should().Be(1L),
            x => x.DocumentModel.Id.Should().Be(2L),
            x => x.Relevance.Should().Be(0)
        );
        nextResult.Should().HaveCount(2)
            .And.AllSatisfy(x => x.Relevance.Should().Be(0));
    }

    [Fact]
    public async Task GetAll_WithLimitAndOffset_ReturnsResultsInConsistentOrder()
    {
        //Arrange
        List<string> query = new() { "er", "i", "unge", "vaccine", "danmark", "kø", "døde", "dronningen", "organsvigt" };
        const int limit = 3;
        const int firstOffset = 0,
            nextOffset = 3;

        //Act
        IEnumerable<SearchResponseModel> firstResult = await _repository.Get(query, new DocumentSearchParameters(), limit, firstOffset);
        IEnumerable<SearchResponseModel> nextResult = await _repository.Get(query, new DocumentSearchParameters(), limit, nextOffset);

        //Assert
        firstResult.Concat(nextResult).Should().HaveCount(5)
            .And.BeInDescendingOrder(x => x.Relevance);
    }
}

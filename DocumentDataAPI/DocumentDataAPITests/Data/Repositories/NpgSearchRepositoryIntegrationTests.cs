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
        _repository = new NpgSearchRepository(connectionFactory, logger, relevanceFunction);
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
        List<long> expected = new()
            { 1, 2, 3, 4, 5 };
        // Explanation: Document 2 contains the word "dronningen" 10 times, which has the highest TF-IDF value.
        // Then, document 1 contains a high TF-IDF valued word "døde" 3 times
        // Finally, the remaining documents contain the word "og" which has a low TF-IdF value, and are ordered by the term frequency of the word.

        // Act
        IEnumerable<SearchResponseModel> result = await _repository.Get(query, new DocumentSearchParameters());
        IEnumerable<long> resultIds = result.Select(d => d.DocumentModel.Id);

        // Assert
        resultIds.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering(), "because document 1 is more relevant than document 2");
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

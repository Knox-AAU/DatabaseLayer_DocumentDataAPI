using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgSimilarDocumentRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgSimilarDocumentRepository _repository;

    public NpgSimilarDocumentRepositoryIntegrationTests()
    {
        NpgDbConnectionFactory connectionFactory = new(DatabaseOptions.ConnectionString);
        ILogger<NpgSimilarDocumentRepository> logger = new Logger<NpgSimilarDocumentRepository>(new NullLoggerFactory());
        _repository = new NpgSimilarDocumentRepository(connectionFactory, logger, new DapperSqlHelper(Configuration));
    }

    [Fact]
    public async Task GetAll_ReturnsAllSimilarDocuments()
    {
        // Arrange

        // Act
        List<SimilarDocumentModel> result = (await _repository.GetAll()).ToList();

        // Assert
        result.Should().HaveCount(6, "because the similar_documents table in the test database has 6 rows");
    }

    [Fact]
    public async Task Get_ReturnsCorrectRows()
    {
        // Arrange
        const long mainDocumentId = 1;

        // Act
        IEnumerable<SimilarDocumentModel> result = await _repository.Get(mainDocumentId);

        // Assert
        result.Should().AllSatisfy(d => { d.MainDocumentId.Should().Be(mainDocumentId); },
            "because the query specifies MainDocumentId");
    }

    [Fact]
    public async Task Add_AddModelCompareResult_ReturnDocumentModel()
    {
        //Arrange
        const long mainDocumentId = 1;
        List<SimilarDocumentModel> similarDocuments = new();
        similarDocuments.Add(new SimilarDocumentModel(mainDocumentId, 4, 69));

        //Act
        IEnumerable<long> id = await _repository.AddBatch(similarDocuments);
        IEnumerable<SimilarDocumentModel> result = await _repository.Get(mainDocumentId);

        //Assert
        result.Should().AllSatisfy(d => { d.MainDocumentId.Should().Be(mainDocumentId); },
            "because the query specifies MainDocumentId");
    }

    [Fact]
    public async Task DeleteAll_RemovesAllSimilarDocuments_ReturnsEmptyTable()
    {
        //Arrange

        //Act
        int id = await _repository.DeleteAll();
        List<SimilarDocumentModel> result = (await _repository.GetAll()).ToList();

        //Assert
        result.Should().HaveCount(0, "because all rows have been deleted");

    }
}

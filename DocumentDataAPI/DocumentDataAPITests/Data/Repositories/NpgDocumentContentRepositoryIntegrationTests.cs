using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgDocumentContentRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;

    public NpgDocumentContentRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgDocumentContentRepository>(new NullLoggerFactory());
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAll_ReturnsAllDocumentContents()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger);

        // Act
        List<DocumentContentModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().HaveCount(5, "because the document_contents table in the test database has 5 rows");
    }

    [Fact]
    public void Update_UpdatesRow()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger);
        const string expected = "Test Content";

        // Act
        repository.Update(new DocumentContentModel(expected, 1));
        string? actual = repository.Get(1).Content;

        // Assert
        actual.Should().BeEquivalentTo(expected,
            "because the content of the document with id 1 was updated to (1, 'Test Content')");
    }

    [Fact]
    public void DeleteWithNoForeignKeyViolation_DeletesRow()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger);
        DocumentContentModel toBeDeleted = repository.Get(5);
        repository.Delete(toBeDeleted);

        // Act
        DocumentContentModel? actual = repository.Get(5);

        // Assert
        actual.Should().BeNull("because the document_content with id 5 was removed from the database");
    }
}

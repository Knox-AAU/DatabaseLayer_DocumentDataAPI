using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgDocumentContentRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentContentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgDocumentContentRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgDocumentContentRepository>(new NullLoggerFactory());
        _sqlHelper = Mock.Of<ISqlHelper>();
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public async Task GetAll_ReturnsAllDocumentContents()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        // Act
        List<DocumentContentModel> result = (await repository.GetAll()).ToList();

        // Assert
        result.Should().HaveCount(5, "because the document_contents table in the test database has 5 rows");
    }

    [Fact]
    public async Task Update_UpdatesRow()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        const string expected = "Test Content";

        // Act
        await repository.Update(new DocumentContentModel(expected, 1));
        string? actual = (await repository.Get(1))?.Content;

        // Assert
        actual.Should().NotBeNull()
            .And.BeEquivalentTo(expected,
            "because the content of the document with id 1 was updated to (1, 'Test Content')");
    }

    [Fact]
    public async Task DeleteWithNoForeignKeyViolation_DeletesRow()
    {
        // Arrange
        NpgDocumentContentRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        DocumentContentModel? toBeDeleted = await repository.Get(5)!;
        await repository.Delete(toBeDeleted);

        // Act
        DocumentContentModel? actual = await repository.Get(5);

        // Assert
        actual.Should().BeNull("because the document_content with id 5 was removed from the database");
    }
}

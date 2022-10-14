using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

public class NpgDocumentRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDocumentRepository> _logger;

    public NpgDocumentRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgDocumentRepository>(new NullLoggerFactory());
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAll_ReturnsAllSources()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);

        // Act
        List<DocumentModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new SourceModel(1, "DR"),
            new SourceModel(2, "TV2")
        }, "because the test database contains (1, 'DR') and (2, 'TV2')");
    }

}
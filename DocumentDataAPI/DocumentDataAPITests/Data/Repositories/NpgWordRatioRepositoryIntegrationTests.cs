using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgWordRatioRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;

    public NpgWordRatioRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new NullLogger<NpgWordRatioRepository>();
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAll_ReturnsAllWordRatios()
    {
        // Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger);

        // Act
        List<WordRatioModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().HaveCount(410, "because the word_ratios table in the test database has 410 rows");
    }
}

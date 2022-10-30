using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgSourceRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgDataSourceRepository> _logger;

    public NpgSourceRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgDataSourceRepository>(new NullLoggerFactory());
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public async Task GetAll_ReturnsAllSources()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);

        // Act
        List<DataSourceModel> result = (await repository.GetAll()).ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new DataSourceModel(1, "DR"),
            new DataSourceModel(2, "TV2")
        }, "because the test database contains (1, 'DR') and (2, 'TV2')");
    }

    [Fact]
    public async Task Update_UpdatesRow()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        const string expected = "Test Source";

        // Act
        await repository.Update(new DataSourceModel(1, expected));
        string? actual = (await repository.Get(1))?.Name;

        // Assert
        actual.Should().NotBeNull()
            .And.Be(expected, "because the tuple (1, 'DR') was updated to (1, 'Test Source')");
    }

    [Fact]
    public async Task DeleteWithNoForeignKeyViolation_DeletesRow()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        IEnumerable<DataSourceModel> expected = new List<DataSourceModel>() { new(1, "DR"), new(2, "TV2") };
        DataSourceModel newDataSource = new(3, "Test");
        await repository.Add(newDataSource);

        // Act
        await repository.Delete(newDataSource);
        IEnumerable<DataSourceModel> actual = await repository.GetAll();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Add_AddsNewSource()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        DataSourceModel expected = new(3, "Test");
        await repository.Add(expected);

        // Act
        DataSourceModel? actual = await repository.Get(expected.Id);

        // Assert
        actual.Should().NotBeNull("because it was added to the database")
            .And.BeEquivalentTo(expected, "because (3, 'Test') was inserted into the database");
    }

    [Fact]
    public async Task GetById_ReturnsSourceSpecifiedById()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        DataSourceModel expected = new(1, "DR");

        // Act
        DataSourceModel? actual = await repository.Get(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected, "because the tuple (1, 'DR') is contained within the database");
    }

    [Fact]
    public async Task GetByName_ReturnsSourcesSpecifiedByName()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        DataSourceModel dataSource = new(1, "DR");
        IEnumerable<DataSourceModel> expected = new List<DataSourceModel>() { dataSource };

        // Act
        IEnumerable<DataSourceModel>? actual = await repository.GetByName(expected.First().Name);

        // Assert
        actual.Should().NotBeNullOrEmpty("because it contains a source with specified name")
            .And.BeEquivalentTo(expected, "because there is one source with name DR");
    }

    [Fact]
    public async Task GetCountFromId_ReturnsCountOfDocumentsFromSource()
    {
        // Arrange
        NpgDataSourceRepository repository = new(_connectionFactory, _logger);
        DataSourceModel dataSource = new(1, "DR");
        const long expected = 3;

        // Act
        long actual = await repository.GetCountFromId(dataSource.Id);

        // Assert
        actual.Should().Be(expected, "because the DR source contains 3 entities");
    }
}

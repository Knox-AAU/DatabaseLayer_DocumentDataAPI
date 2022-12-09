using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgSourceRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgSourceRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgSourceRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(DatabaseOptions);
        _logger = new Logger<NpgSourceRepository>(new NullLoggerFactory());
        _sqlHelper = new DapperSqlHelper(Configuration);
    }

    [Fact]
    public async Task GetAll_ReturnsAllSources()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        // Act
        List<SourceModel> result = (await repository.GetAll()).ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new SourceModel(1, "DR"),
            new SourceModel(2, "TV2")
        }, "because the test database contains (1, 'DR') and (2, 'TV2')");
    }

    [Fact]
    public async Task Update_UpdatesRow()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        const string expected = "Test Source";

        // Act
        await repository.Update(new SourceModel(1, expected));
        string? actual = (await repository.Get(1))?.Name;

        // Assert
        actual.Should().NotBeNull()
            .And.Be(expected, "because the tuple (1, 'DR') was updated to (1, 'Test Source')");
    }

    [Fact]
    public async Task DeleteWithNoForeignKeyViolation_DeletesRow()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        IEnumerable<SourceModel> expected = new List<SourceModel>() { new(1, "DR"), new(2, "TV2") };
        SourceModel newSource = new(3, "Test");
        await repository.Add(newSource);

        // Act
        await repository.Delete(newSource.Id);
        IEnumerable<SourceModel> actual = await repository.GetAll();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Add_AddsNewSource()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        SourceModel expected = new(3, "Test");
        await repository.Add(expected);

        // Act
        SourceModel? actual = await repository.Get(expected.Id);

        // Assert
        actual.Should().NotBeNull("because it was added to the database")
            .And.BeEquivalentTo(expected, "because (3, 'Test') was inserted into the database");
    }

    [Fact]
    public async Task GetById_ReturnsSourceSpecifiedById()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        SourceModel expected = new(1, "DR");

        // Act
        SourceModel? actual = await repository.Get(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected, "because the tuple (1, 'DR') is contained within the database");
    }

    [Fact]
    public async Task GetByName_ReturnsSourcesSpecifiedByName()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        SourceModel source = new(1, "DR");
        IEnumerable<SourceModel> expected = new List<SourceModel>() { source };

        // Act
        IEnumerable<SourceModel>? actual = await repository.GetByName(expected.First().Name);

        // Assert
        actual.Should().NotBeNullOrEmpty("because it contains a source with specified name")
            .And.BeEquivalentTo(expected, "because there is one source with name DR");
    }

    [Fact]
    public async Task GetCountFromId_ReturnsCountOfDocumentsFromSource()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        SourceModel source = new(1, "DR");
        const long expected = 3;

        // Act
        long actual = await repository.GetCountFromId(source.Id);

        // Assert
        actual.Should().Be(expected, "because the DR source contains 3 entities");
    }
}

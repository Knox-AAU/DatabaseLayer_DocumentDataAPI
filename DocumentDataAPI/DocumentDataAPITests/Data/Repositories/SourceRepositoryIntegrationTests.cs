using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

namespace DocumentDataAPITests.Data.Repositories;

public class SourceRepositoryIntegrationTests
{
    private readonly PostgresDbConnectionFactory _connectionFactory;

    public SourceRepositoryIntegrationTests()
    {
        _connectionFactory = new PostgresDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAll_ReturnsAllSources()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory);

        // Act
        List<SourceModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new SourceModel(1, "DR"),
            new SourceModel(2, "TV2")
        }, "because the test database contains (1, 'DR') and (2, 'TV2')");
    }

    [Fact]
    public void Update_UpdatesRow()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory);
        const string expected = "Test Source";

        // Act
        repository.Update(new SourceModel(1, expected));
        string? actual = repository.Get(1).Name;

        // Assert
        actual.Should().Be(expected, "because the tuple (1, 'DR') was updated to (1, 'Test Source')");
    }

    [Fact]
    public void DeleteWithNoForeignKeyVoilation_DeletesRow()
    {
        // Arrange
        NpgSourceRepository repository = new(_connectionFactory);
        SourceModel expected = new(3, "Test");
        repository.Add(expected);

        // Act
        SourceModel actual = repository.Get(expected.Id);

        // Assert
        actual.Should().NotBeNull("because it was added to the database")
            .And.BeEquivalentTo(expected, "because (3, 'Test') was inserted into the database");
    }
}

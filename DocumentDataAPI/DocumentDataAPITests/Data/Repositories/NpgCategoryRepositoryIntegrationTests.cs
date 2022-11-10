using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgCategoryRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgCategoryRepository> _logger;

    public NpgCategoryRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgCategoryRepository>(new NullLoggerFactory());
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public async Task GetAll_ReturnsAllCategories()
    {
        // Arrange
        NpgCategoryRepository repository = new(_connectionFactory, _logger);

        // Act
        IEnumerable<CategoryModel> result = await repository.GetAll();

        // Assert
        result.Should().SatisfyRespectively(
            x => x.Name.Should().Be("Uncategorized"),
            y => y.Name.Should().Be("Nyhedsartikel"));
    }

    [Fact]
    public async Task Update_UpdatesRow()
    {
        // Arrange
        NpgCategoryRepository repository = new(_connectionFactory, _logger);
        const string expected = "Test Category";
        CategoryModel category = new()
        {
            Id = 1,
            Name = expected
        };

        // Act
        await repository.Update(category);
        CategoryModel? result = await repository.Get(category.Id);

        // Assert
        result.Should().NotBeNull()
            .And.Subject.As<CategoryModel>()
            .Name.Should().Be(expected, "because the tuple (1, 'Uncategorized') was updated to (1, 'Test Category')");
    }

    [Fact]
    public async Task DeleteWithNoForeignKeyViolation_DeletesRow()
    {
        // Arrange
        NpgCategoryRepository repository = new(_connectionFactory, _logger);
        const int id = 1;
        await repository.Delete(id);

        // Act
        CategoryModel? result = await repository.Get(id);

        // Assert
        result.Should().BeNull("because the category with id 1 was deleted");
    }

    [Fact]
    public async Task Add_AddsNewCategory()
    {
        // Arrange
        NpgCategoryRepository repository = new(_connectionFactory, _logger);
        const string name = "Test Category";
        CategoryModel category = new(name);
        long id = await repository.Add(category);

        // Act
        CategoryModel? actual = await repository.Get((int)id);

        // Assert
        actual.Should().NotBeNull("because it was added to the database")
            .And.Subject.As<CategoryModel>()
            .Name.Should().Be(name, "because a category with this name was inserted into the database");
    }

    [Fact]
    public async Task GetById_ReturnsCategorySpecifiedById()
    {
        // Arrange
        NpgCategoryRepository repository = new(_connectionFactory, _logger);
        const string expected = "Uncategorized";

        // Act
        CategoryModel? actual = await repository.Get(1);

        // Assert
        actual.Should().NotBeNull().And.Subject.As<CategoryModel>()
            .Name.Should().Be(expected, "because the tuple (1, 'Uncategorized') is contained within the database");
    }
}

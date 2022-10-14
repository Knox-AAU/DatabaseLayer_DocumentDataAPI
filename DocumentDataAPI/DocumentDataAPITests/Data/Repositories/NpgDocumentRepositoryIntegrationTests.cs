using DocumentDataAPI.Controllers;
using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
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
    public void GetAll_ReturnsAllDocuments()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);

        // Act
        List<DocumentModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().HaveCount(5, "because the documents table in the test database has 5 rows");
    }

    [Fact]
    public void GetAll_SearchParametersByAuthorReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentSearchParameters parameters = new();
        string searchAuthor = "Maja Lærke Maach";
        parameters.AddAuthor(searchAuthor);

        // Act
        List<DocumentModel> result = repository.GetAll(parameters).Result.ToList();

        // Assert
        result.Should().AllSatisfy(d =>
        {
            d.Author.Should().Be(searchAuthor);
        }, "because the query specifies an author");
    }

    [Fact]
    public void GetAll_SearchParametersByAuthorAndDateReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentSearchParameters parameters = new();
        const string searchAuthor = "Maja Lærke Maach";
        DateTime searchDate = DateTime.Parse("2022-10-07 13:34:00.000");
        parameters.AddAuthor(searchAuthor)
            .AddAfterDate(searchDate);

        // Act
        List<DocumentModel> result = repository.GetAll(parameters).Result.ToList();

        // Assert
        result.Should().AllSatisfy(d =>
        {
            d.Author.Should().Be(searchAuthor);
            d.Date.Should().BeAfter(searchDate);
        }, "because the query specifies an author and afterdate");
    }

    [Fact]
    public void Get_ReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        int id = 1;

        // Act
        DocumentModel result = repository.Get(id).Result;


        // Assert
        result.Id.Should().Be(1);
            
    /*        (d =>
        {
            d.id.Should().Be(1);
        }, "because the query specifies a specific id (In this case 1)");*/

    }

    [Fact]
    public void Get_ReturnsDefault()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        int id = 9999; //out of bounds

        // Act
        var result = repository.Get(id).result;


        // Assert
        result.ShouldBe(null);
    }
}

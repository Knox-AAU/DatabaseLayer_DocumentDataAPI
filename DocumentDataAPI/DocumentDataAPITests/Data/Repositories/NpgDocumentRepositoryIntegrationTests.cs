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
        List<DocumentModel> result = repository.GetAll(parameters).ToList();

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
        List<DocumentModel> result = repository.GetAll(parameters).ToList();

        // Assert
        result.Should().AllSatisfy(d =>
        {
            d.Author.Should().Be(searchAuthor);
            d.Date.Should().BeAfter(searchDate);
        }, "because the query specifies an author and afterdate");
    }
}

using DocumentDataAPI.Controllers;
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
    public void GetAll_SearchParametersByAuthorReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        SearchParameters parameters = new();
        parameters.AddAuthor("Maja Lærke Maach");

        // Act
        List<DocumentModel> result = repository.GetAll(parameters).ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new DocumentModel("Maja Lærke Maach", DateTime.Parse("2022-10-07 13:40:00.000"), 1, "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt", 1, "", "Iran hævder, at Mahsa Amini døde af organsvigt", 0),
            new DocumentModel("Maja Lærke Maach", DateTime.Parse("2022-10-07 13:33:00.000"), 2, "https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot", 1, "", "Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot", 0),
        }, "because the test database contains only those articles by given author");
    }

    [Fact]
    public void GetAll_SearchParametersByAuthorAndDateReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        SearchParameters parameters = new();
        parameters.AddAuthor("Maja Lærke Maach")
            .AddAfterDate(DateTime.Parse("2022-10-07 13:34:00.000"));

        // Act
        List<DocumentModel> result = repository.GetAll(parameters).ToList();

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new DocumentModel("Maja Lærke Maach", DateTime.Parse("2022-10-07 13:40:00.000"), 1, "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt", 1, "", "Iran hævder, at Mahsa Amini døde af organsvigt", 0),
        }, "because the test database contains only this one article by given author after given date");
    }
}

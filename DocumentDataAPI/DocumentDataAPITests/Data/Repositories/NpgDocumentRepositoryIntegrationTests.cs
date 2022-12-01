using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgDocumentRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgDocumentRepository _repository;

    public NpgDocumentRepositoryIntegrationTests()
    {
        NpgDbConnectionFactory connectionFactory = new(DatabaseOptions.ConnectionString);
        ILogger<NpgDocumentRepository> logger = new Logger<NpgDocumentRepository>(new NullLoggerFactory());
        _repository = new NpgDocumentRepository(connectionFactory, logger, new DapperSqlHelper(Configuration));
    }

    private static List<DocumentModel> DocumentData =>
        new List<DocumentModel>
        {
            new ("author one", DateTime.Parse("01-01-2020 12:30:00"), 0, "/path/to/document/document", 1, "summary one", "title one", 123456, 1, null, 0),
            new ("author two", DateTime.Parse("02-02-2020 13:40:00"), 0, "/path/to/document/secondDocument", 2, "summary two", "title two", 550, 1, null, 0),
            new ("author three", DateTime.Parse("03-03-2020 14:50:00"), 0, "path/to/document/thirdDocument", 1, "summary three", "title three", 600000, 1, null, 0),
            new ("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:40:00"), 1, "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt", 1, null, "Iran hævder, at Mahsa Amini døde af organsvigt", 0, 2, null, 0),
            new ("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:33:00"), 2, "https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot", 1, null, "Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot", 0, 2, null, 0),
            new ("Mette Stender Pedersen", DateTime.Parse("2022-10-07 05:01:00"), 4, "https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920", 2, null, "Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt", 0, 2, null, 0),
            new ("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 5, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, null, "Folk stod i timelange køer for vacciner", 0, 2, null, 0),
            new ("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 1234, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, null, "Folk stod i timelange køer for vacciner", 0, 2, null, 0),
        };

    [Fact]
    public async Task Get_ReturnsCorrectRows()
    {
        // Arrange
        DocumentModel model = DocumentData[3];
        const int id = 1;

        // Act
        DocumentModel? result = await _repository.Get(id);

        // Assert
        result.Should().NotBeNull()
            .And.Subject.As<DocumentModel>().Id.Should().Be(id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9999999)]
    public async Task Get_OnIncorrectValue_ReturnsNull(int id)
    {
        // Arrange

        // Act
        DocumentModel? result = await _repository.Get(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ReturnsAllDocuments()
    {
        // Arrange

        // Act
        List<DocumentModel> result = (await _repository.GetAll()).ToList();

        // Assert
        result.Should().HaveCount(5, "because the documents table in the test database has 5 rows");
    }

    [Fact]
    public async Task GetAll_SearchParametersByAuthorReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const string searchAuthor = "Maja Lærke Maach";
        List<string> searchAuthors = new() { searchAuthor };
        parameters.AddAuthors(searchAuthors);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.Author.Should().Be(searchAuthor); },
            "because the query specifies an author");
    }

    [Fact]
    public async Task GetAll_SearchParametersBySeveralAuthors_ReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const string searchAuthor1 = "Maja Lærke Maach";
        const string searchAuthor2 = "Andreas Nygaard Just";
        List<string> searchAuthors = new() { searchAuthor1, searchAuthor2 };
        parameters.AddAuthors(searchAuthors);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.Author.Should().BeOneOf(searchAuthor1, searchAuthor2); },
            "because the query specifies two authors, therefore all documents returned must be authored by either author.");
    }

    [Fact]
    public async Task GetAll_SearchParametersByAllCategories_ReturnsAllRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const int searchCategory1 = 1;
        const int searchCategory2 = 2;
        List<int> searchCategories = new() { searchCategory1, searchCategory2 };
        parameters.AddCategories(searchCategories);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.CategoryId.Should().BeOneOf(searchCategory1, searchCategory2); },
            "because the query specifies two categories, therefore all documents returned must be either category 1 or 2").And.HaveCount(5, "because there are 5 documents in the database, all with category 2");
    }

    [Fact]
    public async Task GetAll_SearchParametersByAllSources_ReturnsAllRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const int searchSource1 = 1;
        const int searchSource2 = 2;
        List<long> searchSources = new() { searchSource1, searchSource2 };
        parameters.AddSources(searchSources);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.SourceId.Should().BeOneOf(searchSource1, searchSource2); },
            "because the query specifies two sources, therefore all documents returned must be either source 1 or 2").And.HaveCount(5, "because there are 5 documents in the database, 3 with source 1 and 2 with source 2");
    }

    [Fact]
    public async Task GetAll_SearchParametersByAuthorAndDateReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const string searchAuthor = "Maja Lærke Maach";
        List<string> searchAuthors = new() { searchAuthor };
        DateTime searchDate = DateTime.Parse("2022-10-07 13:34:00.000");
        parameters.AddAuthors(searchAuthors)
            .AddAfterDate(searchDate);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d =>
        {
            d.Author.Should().Be(searchAuthor);
            d.Date.Should().BeAfter(searchDate);
        }, "because the query specifies an author and afterdate");
    }

    [Fact]
    public async Task Update_UpdatesRow()
    {
        // Arrange
        DocumentCategoryModel model = new(1, 1);

        // Act
        await _repository.UpdateCategory(model);
        DocumentModel? result = await _repository.Get(model.DocumentId);

        // Assert
        result.Should().NotBeNull().And.Match<DocumentModel>(x => x.CategoryId == model.CategoryId,
            "because the category was updated");
    }

    [Fact]
    public async Task Add_AddsRowsCorrect_ReturnInsertedId()
    {
        //Arrange
        DocumentModel model = DocumentData[0];

        //Act
        long id = await _repository.Add(model);

        //Assert
        id.Should().Be(6L, "because the next ID value is 6");
    }

    [Fact]
    public async Task Add_AddModelCompareResult_ReturnDocumentModel()
    {
        //Arrange
        DocumentModel model = DocumentData[0];

        //Act
        long id = await _repository.Add(model);
        DocumentModel? result = await _repository.Get(id);

        //Assert
        result.Should().NotBeNull()
            .And.Match<DocumentModel>(x => x.Id == id && x.Author == model.Author && x.Title == model.Title);
    }

    [Fact]
    public async Task AddBatch_AddsRowsCorrect_ReturnInsertedIds()
    {
        //Arrange
        List<DocumentModel> documentModels = new()
        {
            DocumentData[0],
            DocumentData[1],
            DocumentData[2]
        };

        //Act
        IEnumerable<long> result = await _repository.AddBatch(documentModels);

        //Assert
        result.Should().HaveCount(3, "because the AddBatch method should insert exactly 3 rows in the database");
    }

    [Fact]
    public async Task AddBatch_CompareAddedDocuments_ReturnInsertedIds()
    {
        //Arrange
        List<DocumentModel> documentModels = new()
        {
            DocumentData[0],
            DocumentData[1],
            DocumentData[2]
        };

        //Act
        List<long> ids = (await _repository.AddBatch(documentModels)).ToList();
        List<DocumentModel?> result = new()
        {
            await _repository.Get(ids[0]),
            await _repository.Get(ids[1]),
            await _repository.Get(ids[2])
        };

        //Assert
        result.Should().NotBeNullOrEmpty()
            .And.HaveCount(3);
    }

    [Fact]
    public async Task Delete_RemovesDocumentCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel model = DocumentData[0];
        long id = await _repository.Add(model);

        //Act
        int result = await _repository.Delete(id);

        //Assert
        result.Should().Be(1, "because the add method should only update 1 row in the database");
    }

    [Fact]
    public async Task Delete_RemovesDocumentCorrect_ReturnNull()
    {
        //Arrange
        DocumentModel model = DocumentData[0];
        long id = await _repository.Add(model);

        //Act
        _ = await _repository.Delete(id);
        DocumentModel? result = await _repository.Get(id);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTotalDocumentCount_GetTheNumberOfDocuments_ReturnInt()
    {
        //Arrange
        const int totalCount = 5;

        //Act
        int result = await _repository.GetTotalDocumentCount();

        //Assert
        result.Should().Be(totalCount);
    }

    [Fact]
    public async Task GetAuthors_GetAllAuthors_ReturnList()
    {
        //Arrange
        List<string> authors = new()
        {
            "Andreas Nygaard Just",
            "Mette Stender Pedersen",
            "Maja Lærke Maach",
            "Jonathan Kjær Troelsen"
        };

        //Act
        List<string> result = (await _repository.GetAuthors()).ToList();

        //Assert
        result.Should().BeEquivalentTo(authors);
    }

    [Theory] // The test data contains 5 documents in total.
    [InlineData(0, null, 5)]
    [InlineData(null, null, 5)]
    [InlineData(2, null, 2)]
    [InlineData(2, 4, 1)]
    [InlineData(1, 6, 0)]
    public async Task GetAll_WithVariousLimitAndOffsets_ReturnsExpectedAmounts(int? limit, int? offset, int expected)
    {
        //Act
        IEnumerable<DocumentModel> result = await _repository.GetAll(limit, offset);

        //Assert
        result.Count().Should().Be(expected);
    }
}

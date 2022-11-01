using System.Runtime.InteropServices.ComTypes;
using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgDocumentRepositoryIntegrationTests
{
    private readonly NpgDocumentRepository _repository;

    public NpgDocumentRepositoryIntegrationTests()
    {
        NpgDbConnectionFactory connectionFactory = new(TestHelper.DatabaseOptions.ConnectionString);
        ILogger<NpgDocumentRepository> logger = new Logger<NpgDocumentRepository>(new NullLoggerFactory());
        _repository = new NpgDocumentRepository(connectionFactory, logger, new DapperSqlHelper(TestHelper.Configuration));
        TestHelper.DeployDatabaseWithTestData();
    }

    private static List<DocumentModel> DocumentData =>
        new List<DocumentModel>
        {
            new ("author one", DateTime.Parse("01-01-2020 12:30:00"), (long)1234, "/path/to/document/document", 1, "summary one", "title one", 123456, 1, null, 0),
            new ("author two", DateTime.Parse("02-02-2020 13:40:00"), (long)2345, "/path/to/document/secondDocument", 2, "summary two", "title two", 550, 1, null, 0),
            new ("author three", DateTime.Parse("03-03-2020 14:50:00"), (long)3456, "path/to/document/thirdDocument", 1, "summary three", "title three", 600000, 1, null, 0),
            new ("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:40:00"), 1, "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt", 1, null, "Iran hævder, at Mahsa Amini døde af organsvigt", 0, 1, null, 0),
            new ("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:33:00"), 2, "https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot", 1, null, "Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot", 0, 1, null, 0),
            new ("Mette Stender Pedersen", DateTime.Parse("2022-10-07 05:01:00"), 4, "https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920", 2, null, "Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt", 0, 1, null, 0),
            new ("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 5, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, null, "Folk stod i timelange køer for vacciner", 0, 1, null, 0),
            new ("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 1234, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, null, "Folk stod i timelange køer for vacciner", 0, 1, null, 0),
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
            .And.BeEquivalentTo(model);
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
        parameters.AddAuthor(searchAuthor);

        // Act
        List<DocumentModel> result = (await _repository.GetAll(parameters)).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.Author.Should().Be(searchAuthor); },
            "because the query specifies an author");
    }

    [Fact]
    public async Task GetAll_SearchParametersByAuthorAndDateReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const string searchAuthor = "Maja Lærke Maach";
        DateTime searchDate = DateTime.Parse("2022-10-07 13:34:00.000");
        parameters.AddAuthor(searchAuthor)
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
    public async Task Add_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel model = DocumentData[0];

        //Act
        long result = await _repository.Add(model);

        //Assert
        result.Should().Be(1L, "because the add method should only update 1 row in the database");
    }

    [Fact]
    public async Task Add_AddModelCompareResult_ReturnDocumentModel()
    {
        //Arrange
        DocumentModel model = DocumentData[0];

        //Act
        _ = await _repository.Add(model);
        DocumentModel? result = await _repository.Get(1234);

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(model);
    }

    [Fact]
    public async Task AddBatch_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        List<DocumentModel> documentModels = new()
        {
            DocumentData[0],
            DocumentData[1],
            DocumentData[2]
        };



        //Act
        int result = await _repository.AddBatch(documentModels);

        //Assert
        result.Should().Be(3, "because the AddBatch method should update exactly 3 rows in the database");
    }

    [Fact]
    public async Task AddBatch_CompareAddedDocuments_ReturnRowsAffected()
    {
        //Arrange
        List<DocumentModel> documentModels = new()
        {
            DocumentData[0],
            DocumentData[1],
            DocumentData[2]
        };

        //Act
        _ = await _repository.AddBatch(documentModels);
        List<DocumentModel?> result = new()
        {
            await _repository.Get(1234),
            await _repository.Get(2345),
            await _repository.Get(3456)
        };

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(documentModels);
    }

    [Fact]
    public async Task Delete_RemovesDocumentCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel model = DocumentData[0];
        await _repository.Add(model);

        //Act
        int result = await _repository.Delete(model);

        //Assert
        result.Should().Be(1, "because the add method should only update 1 row in the database");
    }

    [Fact]
    public async Task Delete_RemovesDocumentCorrect_ReturnNull()
    {
        //Arrange
        DocumentModel model = DocumentData[0];
        await _repository.Add(model);

        //Act
        _ = await _repository.Delete(model);
        DocumentModel? result = await _repository.Get(model.Id);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update_UpdatesRowCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel modelOld = DocumentData[0];
        DocumentModel modelNew = DocumentData[7];

        await _repository.Add(modelOld);

        //Act
        _ = await _repository.Update(modelNew);
        DocumentModel? result = await _repository.Get(1234);

        //Assert
        result.Should().NotBeNull()
              .And.BeEquivalentTo(modelNew);
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
}

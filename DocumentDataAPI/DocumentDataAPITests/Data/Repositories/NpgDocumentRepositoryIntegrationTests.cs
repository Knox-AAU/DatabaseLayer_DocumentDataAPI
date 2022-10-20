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
    private readonly NpgDocumentRepository _repository;

    public NpgDocumentRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgDocumentRepository>(new NullLoggerFactory());
        _repository = new NpgDocumentRepository(_connectionFactory, _logger);
        TestHelper.DeployDatabaseWithTestData();
    }
    
    private static List<DocumentModel> _documentData =>
        new List<DocumentModel>
        {
            new DocumentModel("author one", DateTime.Parse("01-01-2020 12:30:00"), (long)1234, "/path/to/document/document", 1, "summary one", "title one", 123456),
            new DocumentModel("author two", DateTime.Parse("02-02-2020 13:40:00"), (long)2345, "/path/to/document/secondDocument", 2, "summary two", "title two", 550),
            new DocumentModel("author three", DateTime.Parse("03-03-2020 14:50:00"), (long)3456, "path/to/document/thirdDocument", 1, "summary three", "title three", 600000),
            new DocumentModel("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:40:00"), 1, "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt", 1, "", "Iran hævder, at Mahsa Amini døde af organsvigt", 0),
            new DocumentModel("Maja Lærke Maach", DateTime.Parse("2022-10-07T13:33:00"), 2, "https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot", 1, "", "Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot", 0),
            new DocumentModel("Mette Stender Pedersen", DateTime.Parse("2022-10-07 05:01:00"), 4, "https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920", 2, "", "Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt", 0),
            new DocumentModel("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 5, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, "", "Folk stod i timelange køer for vacciner", 0),
            new DocumentModel("Jonathan Kjær Troelsen", DateTime.Parse("2022-10-03 09:01:00"), 1234, "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner", 2, "", "Folk stod i timelange køer for vacciner", 0),
        };
    
    [Fact]
    public void Get_ReturnsCorrectRows()
    {
        // Arrange
        DocumentModel model = _documentData[3];
        int id = 1;

        // Act
        DocumentModel? result = _repository.Get(id);
        
        // Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(model);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9999999)]
    public void Get_OnIncorrectValue_ReturnsNull(int id)
    {
        // Arrange

        // Act
        var result = _repository.Get(id);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public void GetAll_ReturnsAllDocuments()
    {
        // Arrange

        // Act
        List<DocumentModel> result = _repository.GetAll().ToList();

        // Assert
        result.Should().HaveCount(5, "because the documents table in the test database has 5 rows");
    }

    [Fact]
    public void GetAll_SearchParametersByAuthorReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        string searchAuthor = "Maja Lærke Maach";
        parameters.AddAuthor(searchAuthor);

        // Act
        List<DocumentModel> result = _repository.GetAll(parameters).ToList();

        // Assert
        result.Should().AllSatisfy(d => { d.Author.Should().Be(searchAuthor); },
            "because the query specifies an author");
    }

    [Fact]
    public void GetAll_SearchParametersByAuthorAndDateReturnsCorrectRows()
    {
        // Arrange
        DocumentSearchParameters parameters = new();
        const string searchAuthor = "Maja Lærke Maach";
        DateTime searchDate = DateTime.Parse("2022-10-07 13:34:00.000");
        parameters.AddAuthor(searchAuthor)
            .AddAfterDate(searchDate);

        // Act
        List<DocumentModel> result = _repository.GetAll(parameters).ToList();

        // Assert
        result.Should().AllSatisfy(d =>
        {
            d.Author.Should().Be(searchAuthor);
            d.Date.Should().BeAfter(searchDate);
        }, "because the query specifies an author and afterdate");
    }
    
    [Fact]
    public void Add_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel model = _documentData[0];

        //Act
        int result = _repository.Add(model);

        //Assert
        result.Should().Be(1, "because the add method should only update 1 row in the database");
    }
    
    [Fact]
    public void Add_AddModelCompareResult_ReturnDocumentModel()
    {
        //Arrange
        DocumentModel model = _documentData[0];

        //Act
        _ = _repository.Add(model);
        DocumentModel? result = _repository.Get(1234);

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(model);
    }
    
    [Fact]
    public void AddBatch_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        List<DocumentModel> documentModels = new List<DocumentModel>()
        {
            _documentData[0],
            _documentData[1],
            _documentData[2]
        };
        


        //Act
        int result = _repository.AddBatch(documentModels);

        //Assert
        result.Should().Be(3, "because the AddBatch method should update exactly 3 rows in the database");
    }
    
    [Fact]
    public void AddBatch_CompareAddedDocuments_ReturnRowsAffected()
    {
        //Arrange
        List<DocumentModel> documentModels = new List<DocumentModel>()
        {
            _documentData[0],
            _documentData[1],
            _documentData[2]
        };
        
        //Act
        _ = _repository.AddBatch(documentModels);
        List<DocumentModel?> result = new()
        {
            _repository.Get(1234),
            _repository.Get(2345),
            _repository.Get(3456)
        };

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(documentModels);
    }

    [Fact]
    public void Delete_RemovesDocumentCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel model = _documentData[0];
        _repository.Add(model);

        //Act
        int result = _repository.Delete(model);

        //Assert
        result.Should().Be(1, "because the add method should only update 1 row in the database");
    }
    
    [Fact]
    public void Delete_RemovesDocumentCorrect_ReturnNull()
    {
        //Arrange
        DocumentModel model = _documentData[0];
        _repository.Add(model);

        //Act
        _ = _repository.Delete(model);
        DocumentModel? result = _repository.Get(1234);

        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public void Update_UpdatesRowCorrect_ReturnRowsAffected()
    {
        //Arrange
        DocumentModel modelOld = _documentData[0];
        DocumentModel modelNew = _documentData[7];

        _repository.Add(modelOld);

        //Act
        _ = _repository.Update(modelNew);
        DocumentModel? result = _repository.Get(1234);

        //Assert
        result.Should().NotBeNull()
              .And.BeEquivalentTo(modelNew);
    }
    
    [Fact]
    public void GetByAuthor_GetAllDocumentsByAuthor_ReturnListOfDocuments()
    {
        //Arrange
        DocumentModel model = _documentData[6];
        
        //Act
        IEnumerable<DocumentModel> result = _repository.GetByAuthor("Jonathan Kjær Troelsen");

            //Assert
            result.Should().NotBeNull()
                .And.AllBeEquivalentTo(model);
    }
    
    [Fact]
    public void GetByDate_GetsAllDocumentsByDate_ReturnListOfDocuments()
    {
        //Arrange
        List<DocumentModel> modelList = new List<DocumentModel>()
        {
            _documentData[3],
            _documentData[4],
            _documentData[5]
        };

        //Act
        IEnumerable<DocumentModel> result = _repository.GetByDate(DateTime.Parse("2022-10-07"));

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(modelList);
    }
    
    [Fact]
    public void GetBySource_GetAllDocumentsBySource_ReturnListOfDocuments()
    {
      //Arrange
        List<DocumentModel> modelList = new List<DocumentModel>()
        {
            _documentData[5],
            _documentData[6]
        };
        
        //Act
        IEnumerable<DocumentModel> result = _repository.GetBySource(2);

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(modelList);
    }
    
    [Fact]
    public void GetTotalDocumentCount_GetTheNumberOfDocuments_ReturnInt()
    {
        //Arrange
        int totalCount = 5;
        
        //Act
        int result = _repository.GetTotalDocumentCount();
        
        //Assert
        result.Should().Be(totalCount);
    }
}

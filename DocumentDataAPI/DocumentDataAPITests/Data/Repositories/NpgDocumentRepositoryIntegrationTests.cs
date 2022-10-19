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
    public void Get_ReturnsCorrectRows()
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel model = new("Maja Lærke Maach",
                                  DateTime.Parse("2022-10-07T13:40:00"),
                                  1,
                                  "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt",
                                  1,
                                  "",
                                  "Iran hævder, at Mahsa Amini døde af organsvigt",
                                  0);
        int id = 1;

        // Act
        DocumentModel? result = repository.Get(id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(model);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9999999)]
    public void Get_OnIncorrectValue_ReturnsNull(int id)
    {
        // Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);

        // Act
        var result = repository.Get(id);
        
        // Assert
        result.Should().Be(null);
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
    
    //Add
    [Fact]
    public void Add_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel model = new ("author",
                                    DateTime.Parse("01-01-2020 12:30:00"),
                                    (long)1234,
                                    "/path/to/document/document",
                                    1,
                                    "summary",
                                    "Title",
                                    123456);

        //Act
        int result = repository.Add(model);

        //Assert
        result.Should().Be(1);
    }
    
    //AddBatch
    [Fact]
    public void AddBatch_addsRowsCorrect_ReturnRowsAffected()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel model1 = new ("author one",
                                    DateTime.Parse("01-01-2020 12:30:00"),
                                    (long)1234,
                                    "/path/to/document/document",
                                    1,
                                    "summary one",
                                    "title one",
                                    123456);

        DocumentModel model2 = new ("author two",
                                    DateTime.Parse("02-02-2020 13:40:00"),
                                    (long)2345,
                                    "/path/to/document/secondDocument",
                                    2,
                                    "summary two",
                                    "title two",
                                    550);

        DocumentModel model3 = new ("author three",
                                    DateTime.Parse("03-03-2020 14:50:00"),
                                    (long)3456,
                                    "path/to/document/thirdDocument",
                                    1,
                                    "summary three",
                                    "title three",
                                    600000);

        List<DocumentModel> documentModels = new();
        documentModels.Add(model1);
        documentModels.Add(model2);
        documentModels.Add(model3);


        //Act
        int result = repository.AddBatch(documentModels);

        //Assert
        result.Should().Be(3);
    }

    //Delete
    [Fact]
    public void Delete_RemovesDocumentCorrect_ReturnRowsAffected()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel model = new("author",
                                    DateTime.Parse("01-01-2020 12:30:00"),
                                    (long)1234,
                                    "/path/to/document/document",
                                    1,
                                    "summary",
                                    "Title",
                                    123456);
        repository.Add(model);

        //Act
        int result = repository.Delete(model);

        //Assert
        result.Should().Be(1);
    }
    
    //Update
    [Fact]
    public void Update_UpdatesRowCorrect_ReturnRowsAffected()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel modelOld = new("author",
                                     DateTime.Parse("2020-01-09 02:01:00"),
                                     10,
                                     "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner",
                                     2,
                                     "",
                                     "Folk stod i timelange køer for vacciner",
                                     0);
        DocumentModel modelNew = new("Jonathan Kjær Troelsen",
                                  DateTime.Parse("2022-10-03 09:01:00"),
                                  10,
                                  "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner",
                                  2,
                                  "",
                                  "Folk stod i timelange køer for vacciner",
                                  0);

        repository.Add(modelOld);

        //Act
        _ = repository.Update(modelNew);
        DocumentModel? result = repository.Get(10);

        //Assert
        result.Should().NotBeNull()
              .And.BeEquivalentTo(modelNew);
    }
    
    [Fact]
    public void GetByAuthor_GetAllDocumentsByAuthor_ReturnListOfDocuments()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        DocumentModel model = new("Jonathan Kjær Troelsen",
                                  DateTime.Parse("2022-10-03 09:01:00"),
                                  5,
                                  "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner",
                                  2,
                                  "",
                                  "Folk stod i timelange køer for vacciner",
                                  0);
        
        //Act
        IEnumerable<DocumentModel> result = repository.GetByAuthor("Jonathan Kjær Troelsen");

            //Assert
            result.Should().NotBeNull()
                .And.AllBeEquivalentTo(model);
    }
    
    [Fact]
    public void GetByDate_GetsAllDocumentsByDate_ReturnListOfDocuments()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        List<DocumentModel> modelList = new List<DocumentModel>();
        DocumentModel modelOne = new("Maja Lærke Maach",
                                     DateTime.Parse("2022-10-07T13:40:00"),
                                     1,
                                     "https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt",
                                     1,
                                     "",
                                     "Iran hævder, at Mahsa Amini døde af organsvigt",
                                     0);
        DocumentModel modelTwo = new("Maja Lærke Maach",
                                     DateTime.Parse("2022-10-07T13:33:00"),
                                     2,
                                     "https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot",
                                     1,
                                     "",
                                     "Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot",
                                     0);
        DocumentModel modelThree = new("Mette Stender Pedersen",
                                       DateTime.Parse("2022-10-07 05:01:00"),
                                       4,
                                       "https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920",
                                       2,
                                       "",
                                       "Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt",
                                       0);
        
        modelList.Add(modelOne);
        modelList.Add(modelTwo);
        modelList.Add(modelThree);
        
        //Act
        IEnumerable<DocumentModel> result = repository.GetByDate(DateTime.Parse("2022-10-07"));

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(modelList);
    }
    
    [Fact]
    public void GetBySource_GetAllDocumentsBySource_ReturnListOfDocuments()
    {
      //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        List<DocumentModel> modelList = new List<DocumentModel>();
        DocumentModel modelOne = new("Mette Stender Pedersen",
                                       DateTime.Parse("2022-10-07 05:01:00"),
                                       4,
                                       "https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920",
                                       2,
                                       "",
                                       "Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt",
                                       0);
        DocumentModel modelTwo = new("Jonathan Kjær Troelsen",
                                     DateTime.Parse("2022-10-03 09:01:00"),
                                     5,
                                     "https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner",
                                     2,
                                     "",
                                     "Folk stod i timelange køer for vacciner",
                                     0);
        
        modelList.Add(modelOne);
        modelList.Add(modelTwo);
        
        //Act
        IEnumerable<DocumentModel> result = repository.GetBySource(2);

        //Assert
        result.Should().NotBeNull()
            .And.BeEquivalentTo(modelList);
    }
    
    [Fact]
    public void GetTotalDocumentCount_GetTheNumberOfDocuments_ReturnInt()
    {
        //Arrange
        NpgDocumentRepository repository = new(_connectionFactory, _logger);
        int totalCount = 5;
        
        //Act
        int result = repository.GetTotalDocumentCount();
        
        //Assert
        result.Should().Be(totalCount);
    }
}

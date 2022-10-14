using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

public class NpgWordRatioRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;

    public NpgWordRatioRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgWordRatioRepository>(new NullLoggerFactory());
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAllReturnsAllWordRatios()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);

        //Act
        List<WordRatioModel> results = repository.GetAll().ToList();

        //Assert
        Assert.Equal(409, results.Count()); // 409 is the amount of word ratios in the test data-set
    }

    [Theory]
    [MemberData(nameof(WordRatioData))]
    public void GetByDocumentIdAndWordReturnsCorrectWordRatio(int docID, string word, WordRatioModel expected)
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);

        //Act
        WordRatioModel? result = repository.GetByDocumentIdAndWord(docID, word);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> WordRatioData =>
        new List<object[]>
        {
            new object[] { 5, "kunne", new WordRatioModel(1, 5, 0.52, (Rank)0, "kunne") },
            new object[] { 3, "et", new WordRatioModel(3, 3, 1.96, (Rank)0, "et") },
            new object[] { 4, "sag", new WordRatioModel(1, 4, 2.56, (Rank)0, "sag") },
            new object[] { 2, "dronningen", new WordRatioModel(2, 2, 1.61, (Rank)1, "dronningen") },
        };

    [Fact]
    public void GetByDocumentIdReturnsCorrectCount()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);

        //Act
        List<WordRatioModel> results = repository.GetByDocumentId(1).ToList();

        //Assert
        results.Count().Should().Be(68, "because the document with id=1 has 68 wordratios");
    }

}

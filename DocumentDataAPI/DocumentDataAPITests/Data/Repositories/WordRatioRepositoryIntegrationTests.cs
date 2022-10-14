using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

namespace DocumentDataAPITests.Data.Repositories;

public class WordRatioRepositoryIntegrationTests
{

    private readonly PostgresDbConnectionFactory _connectionFactory;

    public WordRatioRepositoryIntegrationTests()
    {
        _connectionFactory = new PostgresDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        TestHelper.DeployDatabaseWithTestData();
    }

    [Fact]
    public void GetAllReturnsAllWordRatios()
    {
        //Arrange
        WordRatioRepository repository = new WordRatioRepository(_connectionFactory);

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
        WordRatioRepository repository = new WordRatioRepository(_connectionFactory);

        //Act
        WordRatioModel? result = repository.GetByDocumentIdAndWord(docID, word);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> WordRatioData =>
        new List<object[]>
        {
            new object[] { 5, "kunne", new WordRatioModel(1, 5, 0.52, (Ranks)0, "kunne") },
            new object[] { 3, "et", new WordRatioModel(3, 3, 1.96, (Ranks)0, "et") },
            new object[] { 4, "sag", new WordRatioModel(1, 4, 2.56, (Ranks)0, "sag") },
            new object[] { 2, "dronningen", new WordRatioModel(2, 2, 1.61, (Ranks)1, "dronningen") },
        };

    [Fact]
    public void GetByDocumentIdReturnsCorrectCount()
    {
        //Arrange
        WordRatioRepository repository = new WordRatioRepository(_connectionFactory);

        //Act
        List<WordRatioModel> results = repository.GetByDocumentId(1).ToList();

        //Assert
        results.Count().Should().Be(68, "because the document with id=1 has 68 wordratios");
    }

}
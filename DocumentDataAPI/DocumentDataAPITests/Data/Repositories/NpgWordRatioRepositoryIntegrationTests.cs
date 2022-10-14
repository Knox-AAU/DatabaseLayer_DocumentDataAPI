using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgWordRatioRepositoryIntegrationTests
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgWordRatioRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(TestHelper.DatabaseOptions.ConnectionString);
        _logger = new Logger<NpgWordRatioRepository>(new NullLoggerFactory());
        _sqlHelper = Mock.Of<ISqlHelper>();
        TestHelper.DeployDatabaseWithTestData();
    }

    [Theory]
    [MemberData(nameof(WordRatioData))]
    public void GetByDocumentIdAndWordReturnsCorrectWordRatio(int docID, string word, WordRatioModel expected)
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger, _sqlHelper);

        //Act
        WordRatioModel? result = repository.GetByDocumentIdAndWord(docID, word);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> WordRatioData =>
        new List<object[]>
        {
            new object[] { 5, "kunne", new WordRatioModel(1, 5, 0.5199999809265137, (Rank) 0, "kunne") },
            new object[] { 3, "et", new WordRatioModel(3, 3, 1.9600000381469727, (Rank) 0, "et") },
            new object[] { 4, "sag", new WordRatioModel(1, 4, 2.559999942779541, (Rank) 0, "sag") },
            new object[] { 2, "dronningen", new WordRatioModel(2, 2, 1.6100000143051147, (Rank) 1, "dronningen") },
        };

    [Fact]
    public void GetByDocumentIdReturnsCorrectCount()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger, _sqlHelper);

        //Act
        List<WordRatioModel> results = repository.GetByDocumentId(1).ToList();

        //Assert
        results.Should().HaveCount(68, "because the document with id=1 has 68 wordratios");
    }

    [Fact]
    public void GetByWordReturnsCorrectWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);

        //Act
        List<WordRatioModel> results = repository.GetByWord("til").ToList();

        //Assert
        results.Should().BeEquivalentTo(new List<WordRatioModel>()
        {
            new WordRatioModel(2, 1, 2.059999942779541, (Rank) 0, "til"),
            new WordRatioModel(3, 2, 2.4200000762939453, (Rank) 0, "til"),
            new WordRatioModel(2, 3, 1.309999942779541, (Rank) 0, "til"),
            new WordRatioModel(1, 5, 0.5199999809265137, (Rank) 0, "til")
        });
    }

    [Fact]
    public void GetByWordsReturnsCorrectWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);
        List<string> words = new List<string>() { "til", "dronningen" };

        //Act
        List<WordRatioModel> results = repository.GetByWords(words).ToList();

        //Assert
        results.Should().BeEquivalentTo(new List<WordRatioModel>()
        {
            new WordRatioModel(2, 1, 2.059999942779541, (Rank) 0, "til"),
            new WordRatioModel(3, 2, 2.4200000762939453, (Rank) 0, "til"),
            new WordRatioModel(2, 3, 1.309999942779541, (Rank) 0, "til"),
            new WordRatioModel(1, 5, 0.5199999809265137, (Rank) 0, "til"),
            new WordRatioModel(2, 2, 1.6100000143051147, (Rank) 1, "dronningen")
        });
    }

    [Fact]
    public void GetAll_ReturnsAllWordRatios()
    {
        // Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        // Act
        List<WordRatioModel> result = repository.GetAll().ToList();

        // Assert
        result.Should().HaveCount(410, "because the word_ratios table in the test database has 410 rows");
    }

    [Fact]
    public void AddCorrectlyAddsWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);
        WordRatioModel wordRatio = new WordRatioModel(6, 5, 0.41999998688697815, (Rank) 0, "ass");

        //Act
        int result = repository.Add(wordRatio);

        //Assert
        result.Should().Be(1);
        repository.GetByDocumentIdAndWord(5, "ass").Should().BeEquivalentTo(wordRatio);
    }

    [Fact]
    public void AddWordRatiosCorrectlyAddsWordRatios()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);
        List<WordRatioModel> wordRatios = new List<WordRatioModel>()
        {
            new WordRatioModel(6, 5, 0.41999998688697815, (Rank) 0, "ass"),
            new WordRatioModel(2, 3, 0.20999999344348907, (Rank) 2, "babbi"),
            new WordRatioModel(12, 1, 0.10999999940395355, (Rank) 1, "lilo"),
        };

        //Act
        int result = repository.AddWordRatios(wordRatios);

        //Assert
        result.Should().Be(3);
        repository.GetByDocumentIdAndWord(5, "ass").Should().BeEquivalentTo(wordRatios[0]);
        repository.GetByDocumentIdAndWord(3, "babbi").Should().BeEquivalentTo(wordRatios[1]);
        repository.GetByDocumentIdAndWord(1, "lilo").Should().BeEquivalentTo(wordRatios[2]);
    }

    [Fact]
    public void UpdateCorrectlyUpdatesWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);
        WordRatioModel wordRatio = new WordRatioModel(10, 2, 5, (Rank) 2, "dronningen");

        //Act
        int result = repository.Update(wordRatio);

        //Assert
        result.Should().Be(1);
        repository.GetByDocumentIdAndWord(2, "dronningen").Should().BeEquivalentTo(wordRatio);
    }

    [Fact]
    public void DeleteCorrectlyDeletesWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new NpgWordRatioRepository(_connectionFactory, _logger);
        WordRatioModel wordRatio = new WordRatioModel(2, 2, 1.6100000143051147, (Rank) 1, "dronningen");

        //Act
        int result = repository.Delete(wordRatio);

        //Assert
        result.Should().Be(1);
        repository.GetByDocumentIdAndWord(2, "dronningen").Should().BeNull();
    }
}

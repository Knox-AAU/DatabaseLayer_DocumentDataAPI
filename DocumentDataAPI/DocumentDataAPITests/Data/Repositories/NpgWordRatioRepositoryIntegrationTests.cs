using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgWordRatioRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRatioRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgWordRatioRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(DatabaseOptions);
        _logger = new Logger<NpgWordRatioRepository>(new NullLoggerFactory());
        _sqlHelper = new DapperSqlHelper(Configuration);
    }

    [Theory]
    [MemberData(nameof(WordRatioData))]
    public async Task GetByDocumentIdAndWordReturnsCorrectWordRatio(int docID, string word, WordRatioModel expected)
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        //Act
        WordRatioModel? result = await repository.Get(docID, word);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> WordRatioData =>
        new List<object[]>
        {
            new object[] { 5, "kunne", new WordRatioModel { DocumentId = 5, Percent = 0.5199999809265137, Amount = 1, Rank = Rank.Body, Word = "kunne", TfIdf = 0.8369076837681403F } },
            new object[] { 3, "et", new WordRatioModel { DocumentId = 3, Percent = 1.9600000381469727, Amount = 3, Rank = Rank.Body, Word = "et", TfIdf = 1.0012182420677929F } },
            new object[] { 4, "sag", new WordRatioModel { DocumentId = 4, Percent = 2.559999942779541, Amount = 1, Rank = Rank.Body, Word = "sag", TfIdf = 4.120160963738521F } },
            new object[] { 2, "dronningen", new WordRatioModel { DocumentId = 2, Percent = 1.6100000143051147, Amount = 2, Rank = Rank.Synopsis, Word = "dronningen", TfIdf = 2.591195062042096F } },
        };

    [Fact]
    public async Task GetByDocumentIdReturnsCorrectCount()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        //Act
        List<WordRatioModel> results = (await repository.GetByDocumentId(1)).ToList();

        //Assert
        results.Should().HaveCount(68, "because the document with id=1 has 68 wordratios");
    }

    [Fact]
    public async Task GetByWordReturnsCorrectWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        const string word = "til";

        //Act
        IEnumerable<WordRatioModel> results = await repository.GetByWord(word);

        //Assert
        results.Should().HaveCount(4)
            .And.AllSatisfy(x => x.Word.Should().Be(word));
    }

    [Fact]
    public async Task GetByWordsReturnsCorrectWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        List<string> words = new List<string> { "til", "dronningen" };

        //Act
        List<WordRatioModel> results = (await repository.GetByWords(words)).ToList();

        //Assert
        results.Should().HaveCount(5)
            .And.AllSatisfy(x => x.Word.Should().BeOneOf(words));
    }

    [Fact]
    public async Task GetAll_ReturnsAllWordRatios()
    {
        // Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        // Act
        List<WordRatioModel> result = (await repository.GetAll()).ToList();

        // Assert
        result.Should().HaveCount(410, "because the word_ratios table in the test database has 410 rows");
    }

    [Fact]
    public async Task AddCorrectlyAddsWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        WordRatioModel wordRatio = new(6, 5, 0.41999998688697815, (Rank) 0, "ass");

        //Act
        long result1 = await repository.Add(wordRatio);
        WordRatioModel? result2 = await repository.Get(5, "ass");

        //Assert
        result1.Should().Be(1L);
        result2.Should().BeEquivalentTo(wordRatio);
    }

    [Fact]
    public async Task AddWordRatiosCorrectlyAddsWordRatios()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        List<WordRatioModel> wordRatios = new List<WordRatioModel>
        {
            new(6, 5, 0.41999998688697815, (Rank) 0, "ass"),
            new(2, 3, 0.20999999344348907, (Rank) 2, "babbi"),
            new(12, 1, 0.10999999940395355, (Rank) 1, "lilo"),
        };

        //Act
        int result1 = await repository.AddBatch(wordRatios);
        WordRatioModel? result2 = await repository.Get(5, "ass");
        WordRatioModel? result3 = await repository.Get(3, "babbi");
        WordRatioModel? result4 = await repository.Get(1, "lilo");


        //Assert
        result1.Should().Be(3);
        result2.Should().BeEquivalentTo(wordRatios[0]);
        result3.Should().BeEquivalentTo(wordRatios[1]);
        result4.Should().BeEquivalentTo(wordRatios[2]);
    }

    [Fact]
    public async Task UpdateCorrectlyUpdatesWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        const long documentId = 2L;
        const string word = "dronningen";
        const int expectedAmount = 10;
        WordRatioModel wordRatio = new WordRatioModel { DocumentId = documentId, Percent = 1.6100000143051147, Amount = expectedAmount, Rank = Rank.Synopsis, Word = word, TfIdf = 2.591195062042096F };

        //Act
        int rowsAffected = await repository.Update(wordRatio);
        WordRatioModel? result = await repository.Get(documentId, word);

        //Assert
        rowsAffected.Should().Be(1);
        result.Should().NotBeNull()
            .And.Subject.As<WordRatioModel>()
            .Amount.Should().Be(expectedAmount);
    }

    [Fact]
    public async Task DeleteCorrectlyDeletesWordRatio()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        WordRatioModel wordRatio = new(2, 2, 1.6100000143051147, (Rank) 1, "dronningen");

        //Act
        int result1 = await repository.Delete(wordRatio.DocumentId, wordRatio.Word);
        WordRatioModel? result2 = await repository.Get(2, "dronningen");

        //Assert
        result1.Should().Be(1);
        result2.Should().BeNull();
    }

    [Theory] // The test data contains 410 word_ratios in total.
    [InlineData(0, null, 410)]
    [InlineData(null, null, 410)]
    [InlineData(100, null, 100)]
    [InlineData(500, null, 410)]
    [InlineData(100, 400, 10)]
    public async Task GetAll_WithVariousLimitAndOffsets_ReturnsExpectedAmounts(int? limit, int? offset, int expected)
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);

        //Act
        IEnumerable<WordRatioModel> result = await repository.GetAll(limit, offset);

        //Assert
        result.Count().Should().Be(expected);
    }

    [Fact]
    public async Task GetAll_WithSpecificLimitAndOffset_ReturnsConsistentResults()
    {
        //Arrange
        NpgWordRatioRepository repository = new(_connectionFactory, _logger, _sqlHelper);
        const int limit = 4;
        const int firstOffset = 64,
            nextOffset = 68;

        //Act
        IEnumerable<WordRatioModel> firstResult = await repository.GetAll(limit, firstOffset);
        IEnumerable<WordRatioModel> nextResult = await repository.GetAll(limit, nextOffset);

        //Assert
        firstResult.Should().AllSatisfy(x => x.DocumentId.Should().Be(1L));
        nextResult.Should().AllSatisfy(x => x.DocumentId.Should().Be(2L));
    }
}

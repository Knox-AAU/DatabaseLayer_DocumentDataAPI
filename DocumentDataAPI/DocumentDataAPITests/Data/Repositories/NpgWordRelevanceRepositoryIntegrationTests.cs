using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DocumentDataAPITests.Data.Repositories;

[Collection("DocumentDataApiIntegrationTests")]
public class NpgWordRelevanceRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly NpgDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgWordRelevanceRepository> _relevanceLogger;
    private readonly ILogger<NpgWordRatioRepository> _ratioLogger;
    private readonly ISqlHelper _sqlHelper;

    public NpgWordRelevanceRepositoryIntegrationTests()
    {
        _connectionFactory = new NpgDbConnectionFactory(DatabaseOptions.ConnectionString);
        _relevanceLogger = new Logger<NpgWordRelevanceRepository>(new NullLoggerFactory());
        _ratioLogger = new Logger<NpgWordRatioRepository>(new NullLoggerFactory());
        _sqlHelper = new DapperSqlHelper(Configuration);
    }

    [Fact]
    public async Task UpdateWordRelevancesChangesAllRows()
    {
        //Arrange
        NpgWordRelevanceRepository repository = new(_connectionFactory, _relevanceLogger);

        //Act
        int rowsChanged = await repository.UpdateWordRelevances();

        //Assert
        rowsChanged.Should().Be(410, "there are 410 wordratios in the test database");
    }

    [Theory]
    [InlineData("ikke", 1, 0.68951356)]
    [InlineData("og", 3, 0)]
    [InlineData("mette", 3, 1.04613464)]
    [InlineData("daniel", 4, 4.12016106)]
    public async Task UpdateWordRelevancesCorrectlyCalculatesAndInsertsIntoDB(string word, int docId, float expected)
    {
        //Arrange
        NpgWordRelevanceRepository relevanceRepository = new(_connectionFactory, _relevanceLogger);
        NpgWordRatioRepository ratioRepository = new(_connectionFactory, _ratioLogger, _sqlHelper);

        //Act
        _ = await relevanceRepository.UpdateWordRelevances();
        WordRatioModel? wordRatio = await ratioRepository.Get(docId, word);

        //Assert
        wordRatio.Should().NotBeNull()
            .And.Subject.As<WordRatioModel>()
            .TfIdf.Should().Be(expected);
    }
}

using DocumentDataAPI.Data.Repositories.Helpers;
using Microsoft.Extensions.Configuration;

namespace DocumentDataAPITests.Data.Repositories.Helpers;

public sealed class SqlHelperTests
{
    private readonly IConfiguration _configuration;

    public SqlHelperTests()
    {
        _configuration = new ConfigurationBuilder()
            .Build();
    }

    [Fact]
    public void GetPaginatedQuery_NoArguments_ReturnsOriginalSql()
    {
        // Arrange
        DapperSqlHelper helper = new (_configuration);
        const string sql = "select * from test";

        // Act
        string result = helper.GetPaginatedQuery(sql, null, null, "");

        // Assert
        result.Should().Be(sql);
    }

    [Fact]
    public void GetPaginatedQuery_AllArguments_ReturnsModifiedSql()
    {
        // Arrange
        DapperSqlHelper helper = new (_configuration);
        const string sql = "select * from test";

        // Act
        string result = helper.GetPaginatedQuery(sql, 10, 10, "id");

        // Assert
        result.Should().EndWith("order by id limit 10 offset 10");
    }

    [Fact]
    public void GetPaginatedQuery_OnlyLimit_AddsOnlyOrderingAndLimit()
    {
        // Arrange
        DapperSqlHelper helper = new (_configuration);
        const string sql = "select * from test";

        // Act
        string result = helper.GetPaginatedQuery(sql, 2, null, "id");

        // Assert
        result.Should().EndWith("order by id limit 2");
    }

    [Fact]
    public void GetPaginatedQuery_MultipleOrderingColumns_ReturnsOrderByAllColumnsInOrder()
    {
        // Arrange
        DapperSqlHelper helper = new (_configuration);
        const string sql = "select * from test";

        // Act
        string result = helper.GetPaginatedQuery(sql, 1, null, "id", "word");

        // Assert
        result.Should().EndWith("order by id,word limit 1");
    }
}

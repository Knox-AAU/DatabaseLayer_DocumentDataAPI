using Dapper.FluentMap;
using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Deployment;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentDataAPITests;

public static class TestHelper
{
    private static bool _isInitialized;
    private static IConfiguration? _configuration;

    public static IConfiguration Configuration
    {
        get
        {
            if (!_isInitialized)
            {
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.Tests.local.json"), true)
                    .AddEnvironmentVariables()
                    .Build();
                // Set up Dapper mappers
                FluentMapper.Initialize(config =>
                {
                    config.AddMap(new DocumentContentMap());
                    config.AddMap(new DocumentMap());
                    config.AddMap(new WordRatioMap());
                    config.AddMap(new SourceMap());
                });
                _isInitialized = true;
            }

            return _configuration!;
        }
    }

    public static DatabaseOptions DatabaseOptions =>
        Configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();

    public static void DeployDatabaseWithTestData()
    {
        IDbConnectionFactory connectionFactory = new NpgDbConnectionFactory(DatabaseOptions.ConnectionString);
        DatabaseDeployHelper deployHelper =
            new(Mock.Of<ILogger<DatabaseDeployHelper>>(), Configuration, connectionFactory);
        deployHelper.ExecuteSqlFromFile("deploy_schema.sql");
        deployHelper.ExecuteSqlFromFile("populate_tables.sql");
    }
}

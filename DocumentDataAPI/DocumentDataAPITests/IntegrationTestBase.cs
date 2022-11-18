using Dapper.FluentMap;
using DocumentDataAPI.Data;
using DocumentDataAPI.Data.Deployment;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentDataAPITests;

public abstract class IntegrationTestBase : IDisposable
{
    protected IConfiguration Configuration { get; }
    protected DatabaseOptions DatabaseOptions =>
        Configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();

    protected IntegrationTestBase()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.Tests.local.json"), true)
            .AddEnvironmentVariables()
            .Build();
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new DocumentContentMap());
            config.AddMap(new DocumentMap());
            config.AddMap(new WordRatioMap());
            config.AddMap(new SourceMap());
            config.AddMap(new CategoryMap());
            config.AddMap(new SimilarDocumentMap());
        });
        DeployDatabaseWithTestData();
    }

    private void DeployDatabaseWithTestData()
    {
        IDbConnectionFactory connectionFactory = new NpgDbConnectionFactory(DatabaseOptions.ConnectionString);
        DatabaseDeployHelper deployHelper =
            new(Mock.Of<ILogger<DatabaseDeployHelper>>(), Configuration, connectionFactory);
        deployHelper.ExecuteSqlFromFile("deploy_schema.sql");
        deployHelper.ExecuteSqlFromFile("populate_tables.sql");
    }

    public void Dispose()
    {
        FluentMapper.EntityMaps.Clear();
    }
}

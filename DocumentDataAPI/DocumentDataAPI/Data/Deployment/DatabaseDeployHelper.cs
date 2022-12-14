using System.Data;
using Dapper;
using DocumentDataAPI.Options;

namespace DocumentDataAPI.Data.Deployment;

/// <summary>
/// Helper class for executing SQL scripts on a database.
/// </summary>
public class DatabaseDeployHelper
{
    private readonly ILogger<DatabaseDeployHelper> _logger;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly DatabaseOptions _databaseOptions;

    public DatabaseDeployHelper(ILogger<DatabaseDeployHelper> logger, IConfiguration configuration,
        IDbConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _databaseOptions = configuration.GetRequiredSection(DatabaseOptions.Key).Get<DatabaseOptions>();
    }

    /// <summary>
    /// Reads and executes a SQL script file and replaces the schema placeholder with the schema provided in the application settings.
    /// </summary>
    /// <param name="fileName">File path to SQL script.</param>
    /// <param name="schema">The (optional) schema to execute the SQL script on.</param>
    /// <example>deployHelper.ExecuteSqlFromFile("deploy_schema.sql", DatabaseOptions.Schema.DocumentData)</example>
    public void ExecuteSqlFromFile(string fileName, DatabaseOptions.Schema? schema = null)
    {
        string path = Path.Join(Environment.CurrentDirectory, "Data", "Deployment", "Scripts", fileName);
        try
        {
            string script = File.ReadAllText(path)
                .Replace("${database}", _databaseOptions.Database);

            if (schema.HasValue)
            {
                script = script.Replace("${schema}", _databaseOptions.SchemaToString(schema.Value));
            }
            _logger.LogInformation(script);

            _logger.LogInformation("Executing script: {Path}", path);
            using IDbConnection connection = _connectionFactory.CreateConnection();
            connection.Execute(script);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to execute script:");
            throw;
        }
    }
}

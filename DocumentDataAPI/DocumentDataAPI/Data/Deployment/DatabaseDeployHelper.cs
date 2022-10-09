using System.Data;
using Dapper;
using DocumentDataAPI.Options;

namespace DocumentDataAPI.Data.Deployment;

public class DatabaseDeployHelper
{
    private readonly ILogger<DatabaseDeployHelper> _logger;
    private readonly IServiceProvider _provider;
    private readonly DatabaseOptions _databaseOptions;

    public DatabaseDeployHelper(IServiceProvider provider)
    {
        _provider = provider;
        _logger = provider.GetRequiredService<ILogger<DatabaseDeployHelper>>();
        _databaseOptions = provider.GetRequiredService<IConfiguration>()
            .GetSection(DatabaseOptions.Key)
            .Get<DatabaseOptions>();
    }

    /// <summary>
    /// Reads and executes a SQL script file and replaces the schema placeholder with the schema provided in the application settings.
    /// </summary>
    /// <param name="fileName">File path to SQL script.</param>
    public void ExecuteSqlFromFile(string fileName)
    {
        string path = Path.Join(Environment.CurrentDirectory, @"Data\Deployment", fileName);
        string script = File.ReadAllText(path)
            .Replace("${schema}", _databaseOptions.Schema);
        ExecuteScript(path, script);
    }

    /// <summary>
    /// Executes the provided SQL script.
    /// </summary>
    /// <param name="path">The path to the script file.</param>
    /// <param name="script">The parsed contents of the file.</param>
    private void ExecuteScript(string path, string script)
    {
        _logger.LogInformation("Executing script: {path}", path);
        IDbConnection connection = _provider.GetRequiredService<IDbConnection>();
        try
        {
            connection.Open();
            connection.Execute(script);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to execute script:");
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
}
using System.Data;
using Dapper;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data.Deployment;

public class DatabaseDeployHelper
{
    private readonly DatabaseOptions _databaseOptions;

    public DatabaseDeployHelper(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Deploy()
    {
        string scriptPath = Path.Combine(Environment.CurrentDirectory, @"Data/Deployment/deploy_schema.sql");
        Console.WriteLine("Executing script: " + scriptPath);
        string[] parameters = { _databaseOptions.Database, _databaseOptions.Schema };
        string script = ParseScript(scriptPath, parameters);

        using IDbConnection db = new NpgsqlConnection(_databaseOptions.ConnectionString);
        db.Execute(script);
    }

    /// <summary>
    /// Reads a SQL script file and inserts docker database- and schema name parameters into correct locations for deployment.
    /// </summary>
    /// <param name="scriptPath">File path to SQL script.</param>
    /// <param name="parameters">Array of docker database- and schema name parameters.</param>
    /// <returns>The parsed file as a string, containing the SQL script with replaced parameters.</returns>
    private string ParseScript(string scriptPath, string[] parameters)
    {
        string script = File.ReadAllText(scriptPath);
        return script.Replace("${db}", parameters[0]).Replace("${schema}", parameters[1]);
    }
}
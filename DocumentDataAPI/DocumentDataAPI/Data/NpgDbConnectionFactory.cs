using System.Data;
using System.Text;
using DocumentDataAPI.Options;
using Npgsql;

namespace DocumentDataAPI.Data;

/// <summary>
/// An <see cref="IDbConnectionFactory"/> for PostgreSQL.
/// </summary>
public class NpgDbConnectionFactory : IDbConnectionFactory
{
    private string _connectionString;
    private readonly DatabaseOptions _databaseOptions;

    public NpgDbConnectionFactory(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
        _connectionString = databaseOptions.ConnectionString;
    }

    public IDbConnection CreateConnection()
    {
        return CreateConnection(_connectionString);
    }

    public IDbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    public IDbConnectionFactory WithSchema(DatabaseOptions.Schema schema)
    {
        _connectionString = new StringBuilder(_connectionString)
            .Append("SearchPath=")
            .Append(_databaseOptions.SchemaToString(schema))
            .Append(';')
            .ToString();

        return this;
    }
}

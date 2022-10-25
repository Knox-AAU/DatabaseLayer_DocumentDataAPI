using System.Data;
using Npgsql;

namespace DocumentDataAPI.Data;

/// <summary>
/// An <see cref="IDbConnectionFactory"/> for PostgreSQL.
/// </summary>
public class NpgDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgDbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return CreateConnection(_connectionString);
    }

    public IDbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }
}

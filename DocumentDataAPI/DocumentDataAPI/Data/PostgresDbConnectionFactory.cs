using System.Data;
using Npgsql;

namespace DocumentDataAPI.Data;

/// <summary>
/// An <see cref="IDbConnectionFactory"/> for PostgreSQL.
/// </summary>
public class PostgresDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public PostgresDbConnectionFactory(string connectionString)
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
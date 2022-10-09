using System.Data;

namespace DocumentDataAPI.Data;

/// <summary>
/// Implementations of this interface are used to create instances of <see cref="IDbConnection"/> for a particular database provider, such as Postgres.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a default database connection.
    /// </summary>
    /// <returns>The connection.</returns>
    IDbConnection CreateConnection();
    
    /// <summary>
    /// Creates a database connection using the given <paramref name="connectionString"/>.
    /// </summary>
    /// <param name="connectionString">A connection string to the database.</param>
    /// <returns>The connection.</returns>
    IDbConnection CreateConnection(string connectionString);
}
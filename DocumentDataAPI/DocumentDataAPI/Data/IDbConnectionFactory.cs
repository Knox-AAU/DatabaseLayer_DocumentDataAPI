using System.Data;
using DocumentDataAPI.Options;

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

    /// <summary>
    /// Adds a search path to the connections, which removes the need to specify a schema in queries.
    /// </summary>
    /// <param name="schema">The schema to use for all connections provided by this factory.</param>
    IDbConnectionFactory WithSchema(DatabaseOptions.Schema schema);
}

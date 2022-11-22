using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories.Helpers;

public interface ISqlHelper
{
    /// <summary>
    /// The maximum number of values to include in a single INSERT statement.
    /// </summary>
    int InsertStatementChunkSize { get; }

    /// <summary>
    /// Builds a string of SQL parameters for multiple models, which can be added to a single INSERT statement in SQL.
    /// The values of the parameters are mapped to the <paramref name="parameterDictionary"/> object.
    /// </summary>
    /// <remarks>This is useful when inserting a large batch of models at a time. E.g., 1000 INSERT statements can be combined into a single INSERT statement, which should be more efficient for the database.</remarks>
    /// <param name="models">The actual models to insert.</param>
    /// <param name="parameterDictionary">A dictionary from parameter names to their values.</param>
    /// <typeparam name="T">The type of the models to insert.</typeparam>
    /// <returns>A string consisting of the parameter part of an INSERT statement, e.g. "(@Property11, @Property21),(@Property12, @Property22)".</returns>
    /// <example>string sqlQuery = "insert into document_contents (documents_id, content) values " + GetBatchInsertParameters(models, out Dictionary params);</example>
    string GetBatchInsertParameters<T>(T[] models, out Dictionary<string, dynamic> parameterDictionary);

    /// <summary>
    /// Builds a string of SQL parameters for document searches, which may involve filters on several different attributes.
    /// This method should be used for each parameter, and the calling method should handle whether the filter should start with "where" or "and".
    /// </summary>
    /// <param name="param">A query parameter containing a key, value and comparison operator</param>
    /// <returns>A filtering condition string for a sql query.</returns>
    /// <example>string sqlQuery = "where " + GetParameterString(param);</example>
    string GetParameterString(QueryParameter param);

    /// <summary>
    /// Adds an ordering, a limit, and an offset at the end of the input <paramref name="sql"/> string, which is used for paginating.
    /// </summary>
    /// <param name="sql">The SQL query string to extend with pagination.</param>
    /// <param name="limit">The max number of rows to fetch.</param>
    /// <param name="offset">The number of rows to skip.</param>
    /// <param name="orderByColumns">A nonempty list of columns to order by, usually the columns that constitute the primary key for the relation. The ordering must always be the same to ensure that the "pages" are consistent.</param>
    /// <returns>The input query extended with pagination.</returns>
    /// <exception cref="ArgumentException">If <paramref name="orderByColumns"/> is empty.</exception>
    string GetPaginatedQuery(string sql, int? limit, int? offset = null, params string[] orderByColumns);
}

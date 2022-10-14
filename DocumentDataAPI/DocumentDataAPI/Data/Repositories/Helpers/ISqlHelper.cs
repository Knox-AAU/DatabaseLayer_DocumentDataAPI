using Dapper;

namespace DocumentDataAPI.Data.Repositories.Helpers;

public interface ISqlHelper
{
    /// <summary>
    /// Builds a string of SQL parameters for multiple models, which can be added to a single INSERT statement in SQL.
    /// The values of the parameters are mapped to the <paramref name="parameterDictionary"/> object.
    /// </summary>
    /// <remarks>This is useful when inserting a large batch of models at a time. E.g., 1000 INSERT statements can be combined into a single INSERT statement, which should be more efficient for the database.</remarks>
    /// <param name="models">The actual models to insert.</param>
    /// <param name="parameterDictionary">A dictionary from parameter names to their values, e.g. for use with DynamicParameters from Dapper.</param>
    /// <typeparam name="T">The type of the models to insert.</typeparam>
    /// <returns>A string consisting of the parameter part of an INSERT statement, e.g. "(@Property11, @Property21),(@Property12, @Property22)".</returns>
    /// <example>string sqlQuery = "insert into document_contents (documents_id, content) values " + GetBatchInsertParameters(models, out Dictionary params);</example>
    string GetBatchInsertParameters<T>(T[] models, out DynamicParameters parameterDictionary);
}

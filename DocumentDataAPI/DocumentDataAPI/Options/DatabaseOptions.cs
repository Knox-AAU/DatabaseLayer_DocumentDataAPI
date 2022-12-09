namespace DocumentDataAPI.Options;

/// <summary>
/// Strongly typed options class for the "Database" section in appsettings.
/// Based on the options pattern (https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0)
/// </summary>
public class DatabaseOptions
{
    public enum Schema
    {
        DocumentData
    }

    public const string Key = "Database";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Database { get; set; } = string.Empty;
    public string DocumentDataSchema { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Builds a connection string from the database options.
    /// For example: "UserID=postgres;Password=postgres;Host=localhost;Port=5432;Database=myDatabase;SearchPath=mySchema"
    /// </summary>
    /// <returns>String used to connect to the database.</returns>
    /// <remarks>The SearchPath property specifies which schema is used by default when executing queries on the database.
    /// This is specific to PostgreSQL, and may be different with other database providers.
    /// See https://www.connectionstrings.com/db2-net-data-provider-db2connection/specifying-schema/ for more information.
    /// </remarks>
    public string ConnectionString =>
        "UserID=" + Username + ";" +
        "Password=" + Password + ";" +
        "Host=" + Host + ";" +
        "Port=" + Port + ";" +
        "Database=" + Database + ";";

    public string SchemaToString(Schema schema)
    {
        return schema switch
        {
            Schema.DocumentData => DocumentDataSchema,
            _ => string.Empty
        };
    }
}

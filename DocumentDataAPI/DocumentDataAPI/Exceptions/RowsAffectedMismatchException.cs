namespace DocumentDataAPI.Exceptions;

/// <summary>
/// This exception will be used when a transaction needs to rollback, since the rows affected does not match the amount
/// of entities that should have been affected in the database due to the query.
/// </summary>
[Serializable]
public class RowsAffectedMismatchException : Exception
{
    public RowsAffectedMismatchException()
    {
    }

    public RowsAffectedMismatchException(string message) : base(message)
    {
    }

    public RowsAffectedMismatchException(string message, Exception inner) : base(message, inner)
    {
    }

    protected RowsAffectedMismatchException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}
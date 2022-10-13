
//This exception will be used when a transaction needs to rollback, since
[System.Serializable]
public class RowsAffectedMismatchException : System.Exception
{
  public RowsAffectedMismatchException() { }
  public RowsAffectedMismatchException(string message) : base(message) { }
  public RowsAffectedMismatchException(string message, System.Exception inner) : base(message, inner) { }
  protected RowsAffectedMismatchException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
namespace DocumentDataAPI.Models;

public class QueryParameter
{
    public readonly string Key;
    public readonly dynamic Value;
    public readonly string ComparisonOperator;

    public QueryParameter(string key, dynamic value)
    {
        Key = key;
        Value = value;
        ComparisonOperator = "=";
    }

    public QueryParameter(string key, DateTime value, string comparisonOperator) : this(key, value)
    {
        ComparisonOperator = comparisonOperator;
    }
}
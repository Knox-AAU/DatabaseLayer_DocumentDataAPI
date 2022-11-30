namespace DocumentDataAPI.Models;

public class QueryParameter
{
    public readonly string ParameterName;
    public readonly string Key;
    public readonly dynamic Value;
    public readonly string ComparisonOperator;

    public QueryParameter(string key, dynamic value, string? parameterName = null)
    {
        ParameterName = parameterName ?? key;
        Key = key;
        Value = value;
        ComparisonOperator = "=";
    }

    public QueryParameter(string key, DateTime value, string comparisonOperator, string? parameterName = null) : this(key, value, parameterName: parameterName)
    {
        ComparisonOperator = comparisonOperator;
    }
}

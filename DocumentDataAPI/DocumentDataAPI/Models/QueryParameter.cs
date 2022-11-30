namespace DocumentDataAPI.Models;

public class QueryParameter
{
    public readonly string AttributeName;
    public readonly string ValueName; // Name used for Dapper parameter string interpolation
    public readonly dynamic Value;
    public readonly string ComparisonOperator;

    public QueryParameter(string attributeName, string valueName, dynamic value)
    {
        AttributeName = attributeName;
        Value = value;
        ValueName = valueName;
        ComparisonOperator = "=";
    }

    public QueryParameter(string attributeName, string valueName, DateTime value, string comparisonOperator) : this(attributeName, valueName, value)
    {
        ComparisonOperator = comparisonOperator;
    }
}

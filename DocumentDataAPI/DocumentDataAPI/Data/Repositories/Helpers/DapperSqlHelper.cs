using System.Collections;
using System.Text;
using DocumentDataAPI.Models;
using DocumentDataAPI.Models.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DocumentDataAPI.Data.Repositories.Helpers;

public class DapperSqlHelper : ISqlHelper
{
    public DapperSqlHelper(IConfiguration configuration)
    {
        InsertStatementChunkSize = configuration.GetValue(nameof(InsertStatementChunkSize), defaultValue: 1000);
    }

    /// <inheritdoc />
    public int InsertStatementChunkSize { get; }

    /// <inheritdoc/>
    public string GetBatchInsertParameters<T>(T[] models, out Dictionary<string, dynamic> parameterDictionary)
    {
        parameterDictionary = new Dictionary<string, dynamic>();
        StringBuilder stringBuilder = new();

        // Get all properties on the given generic class T with the [Required] attribute.
        List<string> properties = typeof(T).GetProperties()
            .Where(x => !x.HasAttribute<ExcludeFromGeneratedInsertStatementAttribute>())
            .Select(x => x.Name)
            .ToList();

        // Go through each model
        for (int i = 0; i < models.Length; i++)
        {
            T model = models[i];

            // For each model, append the string: (@Property1.1, @Property2.1, ..., @PropertyN.1) for model with index 1 and with N properties.
            stringBuilder.Append('(');
            stringBuilder.AppendJoin(',', properties.Select(x => $"@{x}{i}"));
            stringBuilder.Append(')');

            // Add the value of each property of the current model to the DynamicParameters
            foreach (string property in properties)
            {
                // Uses reflection to get "value.property"
                dynamic? propertyValue = typeof(T).GetProperty(property)!.GetValue(model);
                parameterDictionary.Add(property + i, propertyValue);
            }

            bool isLastRow = (i == models.Length - 1);
            if (!isLastRow)
            {
                stringBuilder.Append(',');
            }
        }
        return stringBuilder.ToString();
    }

    public string GetParameterString(QueryParameter param)
    {
        string paramString = $"{param.Key} {param.ComparisonOperator} ";
        if (param.Value is IEnumerable) paramString += $"any(@{param.Key})";
        else paramString += $"@{param.Key}";
        return paramString;
    }
}

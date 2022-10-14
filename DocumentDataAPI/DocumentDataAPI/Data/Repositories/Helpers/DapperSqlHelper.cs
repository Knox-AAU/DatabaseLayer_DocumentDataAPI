using System.Data;
using System.Reflection;
using System.Text;
using Dapper;

namespace DocumentDataAPI.Data.Repositories.Helpers;

public class DapperSqlHelper : ISqlHelper
{
    /// <inheritdoc/>
    public string GetBatchInsertParameters<T>(T[] models, out DynamicParameters parameterDictionary)
    {
        parameterDictionary = new DynamicParameters();
        StringBuilder stringBuilder = new();

        // Get all properties on the given generic class T
        List<string> properties = typeof(T).GetProperties().Select(x => x.Name).ToList();

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
                object propertyValue = typeof(T).GetProperty(property)!.GetValue(model)!;
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
}

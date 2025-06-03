using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace ComputerAPI.Helpers;

/// <summary>
/// Swagger schema filter that converts all property names to snake_case
/// for API documentation.
/// </summary>
public class SnakeCaseDocumentFilter : ISchemaFilter
{
    /// <summary>
    /// Apply snake_case naming to all properties in the Swagger schema
    /// </summary>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null || schema.Properties.Count == 0)
        {
            return;
        }

        // Create a new dictionary with snake_case keys
        var properties = schema.Properties.ToDictionary(
            entry => ToSnakeCase(entry.Key),
            entry => entry.Value
        );

        // Replace the original properties dictionary
        schema.Properties.Clear();
        foreach (var property in properties)
        {
            schema.Properties.Add(property.Key, property.Value);
        }
    }

    /// <summary>
    /// Convert a string from PascalCase or camelCase to snake_case
    /// </summary>
    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Use regex to insert underscore before each capital letter and then convert to lowercase
        var result = Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        return result;
    }
}

using System.Text.Json;

namespace ComputerAPI.Helpers;

/// <summary>
/// Custom JSON property naming policy that converts property names to snake_case.
/// Used for serializing and deserializing JSON requests and responses.
/// </summary>
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    /// Converts a property name to snake_case.
    /// </summary>
    /// <param name="name">The property name to convert.</param>
    /// <returns>The snake_case version of the property name.</returns>
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var result = new System.Text.StringBuilder();
        
        // Add the first character as lowercase
        result.Append(char.ToLowerInvariant(name[0]));
        
        // Iterate through the rest of the string
        for (int i = 1; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                // If the current character is uppercase, add an underscore and the lowercase version
                result.Append('_');
                result.Append(char.ToLowerInvariant(name[i]));
            }
            else
            {
                // Otherwise just add the character as-is
                result.Append(name[i]);
            }
        }
        
        return result.ToString();
    }
}

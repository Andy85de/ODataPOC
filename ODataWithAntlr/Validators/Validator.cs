using System.Reflection;
using Microsoft.Net.Http.Headers;

namespace ODataWithSprache.Validators;

public static class Validator
{
    private static bool ValidatePropertyInResult<TResult>(string property, List<Type>? dataTypeAllowed = null)
    {
        if (string.IsNullOrWhiteSpace(property))
        {
            throw new ArgumentException(nameof(property));
        }

        Type objectType = typeof(TResult);

        PropertyInfo[] properties = objectType.GetProperties();

        List<string> propertiesNames = properties.Select(prop => prop.Name).ToList();

        Type? propertyDateType = objectType.GetProperty(property)?.PropertyType;

        if (dataTypeAllowed == null)
        {
            return propertiesNames.Contains(property);
        }

        return propertyDateType != null
            && propertiesNames.Contains(property)
            && dataTypeAllowed.Contains(propertyDateType);
    }
    
    
    
}

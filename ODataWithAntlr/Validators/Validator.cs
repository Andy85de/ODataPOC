using System.Reflection;

namespace ODataWithSprache.Validators;

/// <summary>
///     The validator contains methods to check if the request for a
///     object ist valid.
/// </summary>
internal static class ValidatorHelper
{
    /// <summary>
    ///     Checks if the retrieved attribute is part of the result object.
    ///     It will also checked if the attribute has the right datatype.
    /// </summary>
    /// <param name="property">The property that should be checked if it is part of the <paramref name="TResult" /></param>
    /// <param name="dataTypeAllowed">The data type that are allowed. If the value is null, all data types are allowed.</param>
    /// <typeparam name="TResult">The object that is the result and the attribute should be part of.</typeparam>
    /// <returns>True if the validation succeeded, otherwise false.</returns>
    /// <exception cref="ArgumentException">
    ///     If the
    ///     <param name="property" />
    ///     is null, empty or contains whitespaces.
    /// </exception>
    internal static bool ValidatePropertyInResult<TResult>(string property, List<Type>? dataTypeAllowed = null)
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

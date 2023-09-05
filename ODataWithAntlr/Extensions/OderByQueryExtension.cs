using System.Text;
using ODataWithSprache.Grammar;
using ODataWithSprache.Models;

namespace ODataWithSprache.Extensions;

/// <summary>
///     This extension is for the order by query and will be executed on a list of the
///     <see cref="SortDirection" />.
/// </summary>
public static class OderByQueryExtension
{
    private const string _AssignOperator = "->>";
    private const string _OrderBy = "ORDER BY";

    /// <summary>
    ///     The method converts out of a list of <see cref="SortedProperty" /> a bsql query that is used
    ///     to order items by a specific property.
    /// </summary>
    /// <param name="oderByProperties">The list containing the <see cref="SortedProperty" /> that are used to order the items.</param>
    /// <param name="variable">The variable is used to specify which column the bjson-objects are stored.</param>
    /// <returns>An order by query that can be used in a bsql query to order the items by special properties.</returns>
    /// <exception cref="ArgumentNullException">If the list <param name="oderByProperties"></param> is null.</exception>
    public static string GetOrderByJsonBQuery(this List<SortedProperty>? oderByProperties, string variable)
    {
        if (oderByProperties == null)
        {
            throw new ArgumentNullException(nameof(oderByProperties));
        }

        if (oderByProperties.Count == 0)
        {
            return string.Empty;
        }

        var orderByQuery = new StringBuilder(_OrderBy);

        foreach (SortedProperty property in oderByProperties)
        {
            if (!string.IsNullOrWhiteSpace(property.PropertyName))
            {
                orderByQuery.Append(
                    $" {variable} {_AssignOperator} \'{property.PropertyName} {(property.Sorted == SortDirection.Ascending ? "asc" : "desc")}\',");
            }
        }

        // clean the last "," after the loop has ended
        // otherwise the syntax will be false
        orderByQuery.Replace(",", "", orderByQuery.Length - 1, 1);

        return orderByQuery.ToString();
    }
}

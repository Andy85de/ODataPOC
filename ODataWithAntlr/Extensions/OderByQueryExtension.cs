using System.Text;
using ODataWithSprache.Grammar;
using ODataWithSprache.Models;

namespace ODataWithSprache.Extensions;

public static class OderByQueryExtension
{
    private const string _AssignOperator = "->>";
    private const string _OrderBy = "ORDER BY";

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
            orderByQuery.Append(
                $" {variable} {_AssignOperator} \'{property.PropertyName} {(property.Sorted == SortDirection.Ascending ? "asc" : "desc")}\',");
        }

        // clean the last "," after the loop has ended
        // otherwise the syntax will be false
        orderByQuery.Replace(",", "", orderByQuery.Length - 1, 1);

        return orderByQuery.ToString();
    }
}

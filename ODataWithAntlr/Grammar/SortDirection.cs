using ODataWithSprache.Models;

namespace ODataWithSprache.Grammar;

/// <summary>
///     The sort direction for the order by clause
///     and is used in the <see cref="SortedProperty"/> object.
/// </summary>
public enum SortDirection
{
    /// <summary>
    ///     There is not oder direction.
    /// </summary>
    None,

    /// <summary>
    ///     The sort direction is ascending.
    /// </summary>
    Ascending,

    /// <summary>
    ///     The sort direction is descending.
    /// </summary>
    Descending
}

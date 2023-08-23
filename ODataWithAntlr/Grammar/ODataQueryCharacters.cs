namespace ODataWithSprache.Grammar;

/// <summary>
///     Starting and ending character for a query.
/// </summary>
public class ODataQueryCharacters
{
    /// <summary>
    ///     The character indicates that the filter string has started.
    /// </summary>
    public const char StartingCharacter = '=';

    /// <summary>
    ///     The character indicated the end of a query.
    /// </summary>
    public const char EndingCharterAmpersand = '&';

    /// <summary>
    ///     Every filter option is used to start with a dollar sign. For example
    ///     $filter or $oderBy. It also is indicated, that a query has ended, because
    ///     a new query starts.
    /// </summary>
    public const char EndingCharterDollar = '$';
}

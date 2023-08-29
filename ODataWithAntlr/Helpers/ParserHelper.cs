namespace ODataWithSprache.Helpers;

/// <summary>
///     The parser helper class to parser the right-hand-side for datetime,
///     string or integer. It enclosed, if needed, the value with quotation
///     marks.
/// </summary>
public static class ParserHelper
{
    /// <summary>
    ///     Parses the string for an integer, double or a datetime.
    ///     For the number literals the string should not be quoted.
    /// </summary>
    /// <param name="rightHandSide">
    ///     The right hand side that should be analysed
    ///     and quoted if needed.
    /// </param>
    /// <returns>The right string for the sql query.</returns>
    /// <exception cref="ArgumentException"> If the <paramref name="rightHandSide" /> is null or empty.</exception>
    public static string ParserRightHandSide(string rightHandSide)
    {
        if (string.IsNullOrWhiteSpace(rightHandSide))
        {
            throw new ArgumentException(null, nameof(rightHandSide));
        }

        if (int.TryParse(rightHandSide, out int _))
        {
            return rightHandSide;
        }

        if (double.TryParse(rightHandSide, out double _))
        {
            return rightHandSide;
        }

        return DateTime.TryParse(rightHandSide, out DateTime dateValue)
            ? $"'{dateValue.ToUniversalTime()}'"
            : $"'{rightHandSide}'";
    }
}

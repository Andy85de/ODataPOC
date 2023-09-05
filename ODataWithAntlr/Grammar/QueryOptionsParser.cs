using System.Text.RegularExpressions;
using Sprache;

namespace ODataWithSprache.Grammar;

/// <summary>
///     The option query parser is used to parse a given filter option
///     from a string (for example $filter, $orderby..).
/// </summary>
public class QueryOptionsParser
{
    private string OdataOptionString { get; }
    private readonly string _RegexPatter;
    private const string _EscapeSequence = "\\";

    /// <summary>
    ///     Creates an object of type <see cref="QueryOptionsParser" />.
    /// </summary>
    /// <param name="odataOptionString">The query option that should parsed form a string.</param>
    public QueryOptionsParser(string odataOptionString)
    {
        OdataOptionString = CreateStartingOdataOption(odataOptionString);
        _RegexPatter = $"(.*)({OdataOptionString})";
    }

    /// <summary>
    ///     Get the filter option from a string.
    /// </summary>
    public Parser<string> PartedQueryForSpecialOption =>
        from staringAnyOdataQuery in Parse.RegexMatch(new Regex(_RegexPatter, RegexOptions.IgnoreCase))
            .Select(p => p.Groups[1].Value)
        from filterQuery in Parse.CharExcept(
                new List<char>
                {
                    QueryCharacters.EndingCharterDollar,
                    QueryCharacters.EndingCharterAmpersand
                })
            .Many()
            .Text()
            .Token()
        from close in Parse.Chars(
                QueryCharacters.EndingCharterAmpersand,
                QueryCharacters.EndingCharterDollar)
            .Optional()
        select filterQuery;

    private static string CreateStartingOdataOption(string odataOptionString)
    {
        return
            $"{_EscapeSequence}{QueryCharacters.EndingCharterDollar}{odataOptionString}{QueryCharacters.StartingCharacter}";
    }
}

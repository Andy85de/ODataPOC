using System.Text.RegularExpressions;
using Sprache;

namespace ODataWithSprache.Grammar;

public class ODataQueryOptionsParser
{
    public string OdataOptionString { get; }
    private readonly string _RegexPatter;
    private const string _EscapeSequence = "\\";

    public ODataQueryOptionsParser(string odataOptionString)
    {
        OdataOptionString = CreateStartingOdataOption(odataOptionString);
        _RegexPatter = $"(.*)({OdataOptionString})";
    }

    public Parser<string> PartedQueryForSpecialOption =>
        from staringAnyOdataQuery in Parse.RegexMatch(new Regex(_RegexPatter, RegexOptions.IgnoreCase))
            .Select(p => p.Groups[1].Value)
        from filterQuery in Parse.CharExcept(
                new List<char>
                {
                    ODataQueryCharacters.EndingCharterDollar,
                    ODataQueryCharacters.EndingCharterAmpersand
                })
            .Many()
            .Text()
            .Token()
        from close in Parse.Chars(
                ODataQueryCharacters.EndingCharterAmpersand,
                ODataQueryCharacters.EndingCharterDollar)
            .Optional()
        select filterQuery;

    private static string CreateStartingOdataOption(string odataOptionString)
    {
        return
            $"{_EscapeSequence}{ODataQueryCharacters.EndingCharterDollar}{odataOptionString}{ODataQueryCharacters.StartingCharacter}";
    }
}

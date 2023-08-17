using System.Text.RegularExpressions;
using Sprache;

namespace ODataWithSprache.Grammar;

public class OdataQueryOptionsParser
{
    private readonly string _OdataOptionString;
    private readonly string _RegexPatter;
    private const string _EscapeSequence = "\\";

    public OdataQueryOptionsParser(string odataOptionString)
    {
        _OdataOptionString = CreateStartingOdataOption(odataOptionString);
        _RegexPatter = $"(.*)({_OdataOptionString})";
    }

    public Parser<string> PartedQueryForSpecialOption =>
        from staringAnyOdataQuery in Parse.RegexMatch(new Regex(_RegexPatter, RegexOptions.IgnoreCase))
            .Select(p => p.Groups[1].Value)
        from filterQuery in Parse.CharExcept(
                new List<char>
                {
                    OdataQueryCharacters._EndingCharterDollar,
                    OdataQueryCharacters._EndingCharterAmperSand
                })
            .Many()
            .Text()
            .Token()
        from close in Parse.Chars(
                OdataQueryCharacters._EndingCharterAmperSand,
                OdataQueryCharacters._EndingCharterDollar)
            .Optional()
        select filterQuery;

    private static string CreateStartingOdataOption(string odataOptionString)
    {
        return
            $"{_EscapeSequence}{OdataQueryCharacters._EndingCharterDollar}{odataOptionString}{OdataQueryCharacters._StartingCharacter}";
    }
}

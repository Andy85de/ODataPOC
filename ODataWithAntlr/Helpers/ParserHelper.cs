using Microsoft.VisualBasic.CompilerServices;

namespace ODataWithSprache.Helpers;

public static class ParserHelper
{
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

        if (DateTime.TryParse(rightHandSide, out DateTime dateValue))
        {
            return $"'{dateValue.ToUniversalTime()}'";
        }

        return $"'{rightHandSide}'";
    }
}

using ODataWithSprache.Grammar;

namespace ODataQueryTests;

public static class OperatorExtensions
{

    public static  OperatorType GetOperatorType(this string @operator)
    {
        return @operator.ToLower() switch
        {
            "le" => OperatorType.LessEqualsOperator,
            "lt" => OperatorType.LessThenOperator,
            "eq" => OperatorType.EqualsOperator,
            "gt" => OperatorType.GreaterThenOperator,
            "ge" => OperatorType.GreaterEqualsOperator,
            "ne" => OperatorType.NotEqualsOperator,
            _ => throw new ArgumentOutOfRangeException(
                nameof(@operator),
                $"Not expected direction value: {@operator}")
        };
    }
}

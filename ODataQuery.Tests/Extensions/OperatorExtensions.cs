using ODataWithSprache.Grammar;
using Xunit.Sdk;

namespace ODataQueryTests.Extensions;

public static class OperatorExtensions
{
    public static OperatorType GetOperatorType(this string @operator)
    {
        if (string.IsNullOrWhiteSpace(@operator))
        {
            throw new ArgumentException(null, nameof(@operator));
        }

        return @operator.ToLower() switch
        {
            "le" => OperatorType.LessEqualsOperator,
            "lt" => OperatorType.LessThenOperator,
            "eq" => OperatorType.EqualsOperator,
            "ne" => OperatorType.NotEqualsOperator,
            "gt" => OperatorType.GreaterThenOperator,
            "ge" => OperatorType.GreaterEqualsOperator,
            _ => throw new ArgumentOutOfRangeException(
                nameof(@operator),
                $"Not expected direction value: {@operator}")
        };
    }

    public static ExpressionCombinator GetExpressionCombinator(this string @operator)
    {
        if (string.IsNullOrWhiteSpace(@operator))
        {
            throw new ArgumentException(null, nameof(@operator));
        }

        return @operator.ToLower() switch
        {
            ODataExpressionCombinator.OrCombinator => ExpressionCombinator.Or,
            ODataExpressionCombinator.AndCombinator => ExpressionCombinator.And,
            _ => throw new ArgumentOutOfRangeException(nameof(@operator), $"Not expected direction value: {@operator}")
        };
    }
}

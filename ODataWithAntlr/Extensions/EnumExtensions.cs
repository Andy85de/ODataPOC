using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Extensions;

public static class EnumExtensions
{
    /// <summary>
    ///     Transform the <see cref="OperatorType" /> to a sql operator.
    /// </summary>
    /// <param name="operatorType">The operator that should transform in the sql operator.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">If the
    ///     <param name="operatorType" />
    ///     is out of range.
    /// </exception>
    public static string ToSqlOperator(this OperatorType operatorType)
    {
        return operatorType switch
        {
            OperatorType.None => "none",
            OperatorType.EqualsOperator => "=",
            OperatorType.NotEqualsOperator => "<>",
            OperatorType.GreaterThenOperator => ">",
            OperatorType.GreaterEqualsOperator => ">=",
            OperatorType.LessThenOperator => "<",
            OperatorType.LessEqualsOperator => "<=",
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expressionCombinator"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToSqlBinaryOperator(this ExpressionCombinator expressionCombinator)
    {
        return expressionCombinator switch
        {
            ExpressionCombinator.None => "None",
            ExpressionCombinator.And => "AND",
            ExpressionCombinator.Or => "OR",
            _ => throw new ArgumentOutOfRangeException(nameof(expressionCombinator), expressionCombinator, null)
        };
    }
}

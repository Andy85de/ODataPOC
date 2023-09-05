using ODataWithSprache.Grammar;

namespace ODataWithSprache.Extensions;

/// <summary>
///     Transform special enums to sql-strings.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Transform the <see cref="OperatorType" /> to a sql operator.
    /// </summary>
    /// <param name="operatorType">The operator that should transform in the sql operator.</param>
    /// <returns>A strings that was mapped to the special sql operator.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the
    ///     <param name="operatorType" />
    ///     is out of range.
    /// </exception>
    public static string ToSqlOperator(this OperatorType operatorType)
    {
        return operatorType switch
        {
            OperatorType.None => "none",
            OperatorType.EqualsOperator => "=",
            OperatorType.NotEqualsOperator => "!=",
            OperatorType.GreaterThenOperator => ">",
            OperatorType.GreaterEqualsOperator => ">=",
            OperatorType.LessThenOperator => "<",
            OperatorType.LessEqualsOperator => "<=",
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };
    }

    /// <summary>
    ///     Transforms the <see cref="ExpressionCombinator" /> to a sql binary operator.
    /// </summary>
    /// <param name="expressionCombinator">The </param>
    /// <returns>A strings that was mapped to the special sql binary operator.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the
    ///     <param name="expressionCombinator" />
    ///     is out of range.
    /// </exception>
    public static string ToSqlOperator(this ExpressionCombinator expressionCombinator)
    {
        return expressionCombinator switch
        {
            ExpressionCombinator.None => "None",
            ExpressionCombinator.And => "AND",
            ExpressionCombinator.Or => "OR",
            _ => throw new ArgumentOutOfRangeException(nameof(expressionCombinator), expressionCombinator, null)
        };
    }

    /// <summary>
    ///     The sql operator is used to transform operator in the right
    ///     way.
    /// </summary>
    /// <param name="option">The operator that is used to filter a query.</param>
    /// <returns>The right SQL-Operator.</returns>
    public static string ToSqlOperator(this FilterOption option)
    {
        return option switch
        {
            FilterOption.None => "None",
            FilterOption.DollarFilter => "WHERE",
            FilterOption.DollarOrderBy => "ORDERBY",
            _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
        };
    }
}

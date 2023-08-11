using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataWithSprache.Grammar;

public sealed class FilterQueryGrammar
{
    private const string _FilterStringStarting = "$filter=";
    private const char _EndingCharterAmperCent = '&';
    private const char _EndingCharterDollar = '$';

    private static Parser<ExpressionCombinator> Operator(string op, ExpressionCombinator onType)
    {
        return Parse.String(op).Token().Return(onType);
    }

    private static readonly Parser<ExpressionCombinator> _and = Operator(
        ODataExpressionCombinator.AndCombinator,
        ExpressionCombinator.And);

    private static readonly Parser<ExpressionCombinator> _or = Operator(
        ODataExpressionCombinator.OrCombinator,
        ExpressionCombinator.Or);
    

    public static Parser<string> WholeFilterQuery =>
        from staringAnyOdataQuery in Parse.IgnoreCase(_FilterStringStarting)
        from filterQuery in Parse.CharExcept(
                new List<char>
                {
                    _EndingCharterDollar,
                    _EndingCharterAmperCent
                })
            .Many()
            .Text()
            .Token()
        from close in Parse.Chars(_EndingCharterDollar, _EndingCharterAmperCent).Optional()
        select filterQuery;

    public static Parser <TreeNode> Query =>
        from queryWithoutFilterString in WholeFilterQuery.AtLeastOnce()
        select new RootNode(OdataFilterOption.DollarFilter, queryWithoutFilterString.First());
    public static Parser<OperatorType> OperatorEnum =>
        Parse.String(ODataExpressionOperators.EqualsOperator).Return(OperatorType.EqualsOperator)
            .Or(Parse.String(ODataExpressionOperators.GreaterThenOperator).Return(OperatorType.GreaterThenOperator))
            .Or(Parse.String(ODataExpressionOperators.GreaterEqualsOperator).Return(OperatorType.GreaterEqualsOperator))
            .Or(Parse.String(ODataExpressionOperators.LessThenOperator).Return(OperatorType.LessThenOperator))
            .Or(Parse.String(ODataExpressionOperators.LessEqualsOperator).Return(OperatorType.LessEqualsOperator));
    
    public static Parser<string> InnerValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from openQuotedValue in Parse.Char('′')
        from innerValue in Parse.CharExcept('′').Many().Text()
        from closeQuotedValue in Parse.Char('′')
        select innerValue;
    
    public static Parser<string> LeftHandSide => Parse.Regex("[A-Za-z]+").Text().Token();
    
    public static Parser<TreeNode> ExpressionNode =>
        from leftHandSide in LeftHandSide
        from operatorExpression in OperatorEnum
        from rightHandSideValue  in InnerValue
        select CreateExpressionNode(leftHandSide, rightHandSideValue, operatorExpression);
        
    public static TreeNode CreateExpressionNode(string leftHandSide, string rightHandType, OperatorType operatorType)
    {
        return new ExpressionNode(leftHandSide, rightHandType,operatorType );
    }
}

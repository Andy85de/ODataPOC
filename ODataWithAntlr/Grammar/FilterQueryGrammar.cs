using System.Text.RegularExpressions;
using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataWithSprache.Grammar;

public sealed class FilterQueryGrammar
{
    private static string? _QueryString;
    
    internal static void SetQueryString(string? queryString)
    {
        _QueryString = queryString;
    }
    
    internal static readonly Parser<ExpressionCombinator> _binaryOperator = Parse
        .String(ODataExpressionCombinator.OrCombinator)
        .Return(ExpressionCombinator.Or)
        .Or(Parse.String(ODataExpressionCombinator.AndCombinator).Return(ExpressionCombinator.And));

    internal static Parser<ExpressionCombinator> BinaryOperatorToParse =>
         Parse.RegexMatch(new Regex($".*{ODataExpressionCombinator.OrCombinator}", RegexOptions.IgnoreCase))
        .Return(ExpressionCombinator.Or)
        .Or(
            Parse.RegexMatch(new Regex($".*{ODataExpressionCombinator.AndCombinator}", RegexOptions.IgnoreCase))
               .Return(ExpressionCombinator.And))
        .Or(Parse.Return(ExpressionCombinator.None));

    internal static TreeNode? _currentTree;
    internal static RootNode? _rootNode;

    public static Parser<TreeNode> ParseFilterQuery =>
        from operatorExpression in BinaryOperatorToParse.Preview()
        from selectionTrying in operatorExpression.GetOrDefault() == ExpressionCombinator.None
            ? RootNodeParsed
            : QueryParse
        select selectionTrying;
    
    internal static Parser<TreeNode> RootNodeParsed =>
        from expressionNodeString in ExpressionNode
        select CreateRootNodeExpression(
            expressionNodeString,
            ODataFilterOption.DollarFilter);

    internal static Parser<TreeNode> QueryParse =>
        Parse.ChainOperator(
            _binaryOperator,
            ExpressionNode,
            CreateBinaryExpressionLeaf);
    
    internal static Parser<OperatorType> OperatorEnum =>
        Parse.String(ODataExpressionOperators.EqualsOperator)
            .Return(OperatorType.EqualsOperator)
            .Or(Parse.IgnoreCase(ODataExpressionOperators.GreaterThenOperator).Return(OperatorType.GreaterThenOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.GreaterEqualsOperator).Return(OperatorType.GreaterEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.LessThenOperator).Return(OperatorType.LessThenOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.LessEqualsOperator).Return(OperatorType.LessEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.NotEqualsOperator).Return(OperatorType.NotEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.EqualsOperator).Return(OperatorType.EqualsOperator));
    
    internal static Parser<string> InnerValue => InnerQuotedValue.Or(InnerNotQuotedValue);

    internal static Parser<string> InnerQuotedValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from openQuotedValue in Parse.Char(QuotedCharacter.SingleQuotedCharacter)
        from innerValue in Parse.CharExcept(QuotedCharacter.SingleQuotedCharacter)
            .Many()
            .Text()
            .Named("QuoatedValue(right-side)")
            .Token()
        from closeQuotedValue in Parse.Char(QuotedCharacter.SingleQuotedCharacter)
        select innerValue;

    internal static Parser<string> InnerNotQuotedValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from innerValueWithoutQuotation in Parse.Number.Text().Token().Named("InnerNotQuotedValue(right-side)")
        select innerValueWithoutQuotation;

    internal static Parser<string> LeftHandSide =>
        Parse.Regex("[A-Za-z]+").Text().Named("LeftHandSideExpression").Token();

    internal static Parser<TreeNode> ExpressionNode =>
        from leftHandSide in LeftHandSide.Token()
        from operatorExpression in OperatorEnum.Token()
        from rightHandSideValue in InnerValue.Token()
        select CreateExpressionNode(leftHandSide, rightHandSideValue, operatorExpression);

    internal static TreeNode CreateExpressionNode(string leftHandSide, string rightHandType, OperatorType operatorType)
    {
        return new ExpressionNode(leftHandSide, rightHandType, operatorType);
    }

    internal static TreeNode CreateBinaryExpressionLeaf(ExpressionCombinator op, TreeNode? left, TreeNode? right)
    {
        if (left == null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right == null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        if (_rootNode == null)
        {
            _rootNode = new RootNode(ODataFilterOption.DollarFilter, _QueryString ?? string.Empty);
            _rootNode.LeftChild = new BinaryExpression(left, _rootNode, op);
            _currentTree = ((BinaryExpression)_rootNode.LeftChild).RightChild;

            return _rootNode;
        }

        var temp = new BinaryExpression(left, right, op);
        _currentTree = temp;
        _currentTree = ((BinaryExpression)_currentTree).RightChild;

        return _rootNode;
    }
    
    internal static TreeNode CreateRootNodeExpression(TreeNode? treeNode, ODataFilterOption option)
    {
        if (treeNode == null)
        {
            throw new ArgumentNullException(nameof(treeNode));
        }

        _rootNode = new RootNode(option, _QueryString ?? string.Empty)
        {
            LeftChild = treeNode
        };

        return _rootNode;
    }
}

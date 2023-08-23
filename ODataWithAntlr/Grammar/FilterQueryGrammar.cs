using System.Text.RegularExpressions;
using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataWithSprache.Grammar;

/// <summary>
///     The filter query grammar is used to parse a filter-Operation string and return
///     a tree that can be traversed.
/// </summary>
public sealed class FilterQueryGrammar
{
    /// <summary>
    ///     The query string is save for debug reasons and can be found at the root of the
    ///     returned tree.
    /// </summary>
    private static string? _QueryString;

    /// <summary>
    ///     The function is used to set a query parameter for debugging reasons.
    /// </summary>
    /// <param name="queryString">The string that should be saved.</param>
    internal static void SetQueryString(string? queryString)
    {
        _QueryString = queryString;
    }

    /// <summary>
    ///     The binary operator are used to parse an expression an return the <see cref="ExpressionCombinator" />.
    /// </summary>
    internal static readonly Parser<ExpressionCombinator> _binaryOperator = Parse
        .String(ODataLikeExpressionCombinator.OrCombinator)
        .Return(ExpressionCombinator.Or)
        .Or(Parse.String(ODataLikeExpressionCombinator.AndCombinator).Return(ExpressionCombinator.And));

    /// <summary>
    ///     The binary expression is used as a look-ahead to evaluated if the string has and binaryExpression.
    ///     If no binary expression exists, the tree has only a root with one leaf.
    /// </summary>
    internal static Parser<ExpressionCombinator> BinaryOperatorAsLookAheadParser =>
        Parse.RegexMatch(new Regex($".*{ODataLikeExpressionCombinator.OrCombinator}", RegexOptions.IgnoreCase))
            .Return(ExpressionCombinator.Or)
            .Or(
                Parse.RegexMatch(new Regex($".*{ODataLikeExpressionCombinator.AndCombinator}", RegexOptions.IgnoreCase))
                    .Return(ExpressionCombinator.And))
            .Or(Parse.Return(ExpressionCombinator.None));

    /// <summary>
    ///     The current position of the tree.
    /// </summary>
    internal static BinaryExpressionNode? _currentTree;

    /// <summary>
    ///     The root of the tree that will be returned.
    /// </summary>
    internal static RootNode? _rootNode;

    /// <summary>
    ///     The binary look-ahead is used to decide if the tree has an only one or more expressions,
    ///     that is combined with a binary operator.
    /// </summary>
    public static Parser<TreeNode> QueryFilterParser =>
        from operatorExpression in BinaryOperatorAsLookAheadParser.Preview()
        from selectionTrying in operatorExpression.GetOrDefault() == ExpressionCombinator.None
            ? RootNodeParsed
            : TreeParser
        select selectionTrying;

    /// <summary>
    ///     This parser os used to created a root with one expression.
    ///     The filter string than has only one expression.
    /// </summary>
    internal static Parser<TreeNode> RootNodeParsed =>
        from expressionNodeString in ExpressionNode
        select CreateRootNodeExpression(
            expressionNodeString,
            ODataFilterOption.DollarFilter);

    /// <summary>
    ///  The parser combines the one or more expressions with a binary operator
    ///  (current supported operators are "and" and "or" operators).
    /// </summary>
    internal static Parser<TreeNode> QueryBinaryParser =>
        Parse.ChainOperator(
            _binaryOperator,
            ExpressionNode,
            CreateBinaryExpressionLeaf);

    internal static Parser<TreeNode> TreeParser =>
        from parser in QueryBinaryParser
        select _rootNode;

    /// <summary>
    ///     The operator enum parser an operator and return the equivalent <see cref="OperatorType" />.
    /// </summary>
    internal static Parser<OperatorType> OperatorEnum =>
        Parse.String(ODataLikeExpressionOperators.EqualsOperator)
            .Return(OperatorType.EqualsOperator)
            .Or(Parse.IgnoreCase(ODataLikeExpressionOperators.GreaterThenOperator).Return(OperatorType.GreaterThenOperator))
            .Or(
                Parse.IgnoreCase(ODataLikeExpressionOperators.GreaterEqualsOperator)
                    .Return(OperatorType.GreaterEqualsOperator))
            .Or(Parse.IgnoreCase(ODataLikeExpressionOperators.LessThenOperator).Return(OperatorType.LessThenOperator))
            .Or(Parse.IgnoreCase(ODataLikeExpressionOperators.LessEqualsOperator).Return(OperatorType.LessEqualsOperator))
            .Or(Parse.IgnoreCase(ODataLikeExpressionOperators.NotEqualsOperator).Return(OperatorType.NotEqualsOperator))
            .Or(Parse.IgnoreCase(ODataLikeExpressionOperators.EqualsOperator).Return(OperatorType.EqualsOperator));

    /// <summary>
    ///     The inner value of an expression that can contain a quoted or unquoted value.
    /// </summary>
    internal static Parser<string> InnerValue => InnerQuotedValue.Or(InnerNotQuotedValue);

    /// <summary>
    ///     The inner value of an expression that contains a quoted value.
    /// </summary>
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

    /// <summary>
    ///     The inner not quoted value that is normally a number.
    /// </summary>
    internal static Parser<string> InnerNotQuotedValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from innerValueWithoutQuotation in Parse.Number.Text().Token().Named("InnerNotQuotedValue(right-side)")
        select innerValueWithoutQuotation;

    /// <summary>
    ///     The left hand side of an expression.
    /// </summary>
    internal static Parser<string> LeftHandSide =>
        Parse.Regex("[A-Za-z]+").Text().Named("LeftHandSideExpression").Token();

    /// <summary>
    ///     An Expression that consists out of a left and right-hand-side an is combined with an operator.
    /// </summary>
    internal static Parser<TreeNode> ExpressionNode =>
        from leftHandSide in LeftHandSide.Token()
        from operatorExpression in OperatorEnum.Token()
        from rightHandSideValue in InnerValue.Token()
        select CreateExpressionNode(leftHandSide, rightHandSideValue, operatorExpression);

    /// <summary>
    ///     Is used to create an expression node that contains a left and right-hand-side
    ///     that is combined by an operator.
    /// </summary>
    /// <param name="leftHandSide">The left hand side of an expression.</param>
    /// <param name="rightHandType">The right hand side of an expression.</param>
    /// <param name="operatorType">The operator that combines the the right adn left-hand-side expression.</param>
    /// <returns>Returns an <see cref="ExpressionNode" />.</returns>
    internal static TreeNode CreateExpressionNode(string leftHandSide, string rightHandType, OperatorType operatorType)
    {
        return new ExpressionNode(leftHandSide, rightHandType, operatorType);
    }

    /// <summary>
    ///     Created an binary expression tree that combines two expression with a binary operator.
    /// </summary>
    /// <param name="binaryOperator">The binary operator that is used to combines the expressions.</param>
    /// <param name="leftExpression">The left hand expression-</param>
    /// <param name="rightExpression">The right hand expression.</param>
    /// <returns>Returns a <see cref="BinaryExpressionNode" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the
    ///     <param name="leftExpression" />
    ///     or
    ///     <param name="rightExpression" />
    ///     are null.
    /// </exception>
    internal static TreeNode CreateBinaryExpressionLeaf(
        ExpressionCombinator binaryOperator,
        TreeNode? leftExpression,
        TreeNode? rightExpression)
    {
        if (leftExpression == null)
        {
            throw new ArgumentNullException(nameof(leftExpression));
        }

        if (rightExpression == null)
        {
            throw new ArgumentNullException(nameof(rightExpression));
        }

        if (_rootNode == null)
        {
            _rootNode = new RootNode(ODataFilterOption.DollarFilter, _QueryString ?? string.Empty)
            {
                LeftChild = new BinaryExpressionNode(leftExpression, rightExpression, binaryOperator)
            };

            _currentTree = (BinaryExpressionNode)_rootNode.LeftChild;

            return _currentTree;
        }

        var binaryExpression = new BinaryExpressionNode(
            _currentTree!.RightChild,
            rightExpression,
            binaryOperator);

        _currentTree.RightChild = binaryExpression;
        _currentTree = binaryExpression;

        return _currentTree;
    }

    /// <summary>
    ///     Creates a root node that only has one leaf/child as expression.
    /// </summary>
    /// <param name="treeNode">The tree node expression that will append to the root.</param>
    /// <param name="option">The filter string option that is used for the </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="treeNode" /> is null.</exception>
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

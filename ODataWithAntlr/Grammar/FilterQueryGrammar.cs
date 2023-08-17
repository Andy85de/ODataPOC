using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataWithSprache.Grammar;

public sealed class FilterQueryGrammar
{
    public static readonly string ContainsCombinatorExpression =
        $"(.*){ODataExpressionCombinator.OrCombinator}|{ODataExpressionCombinator.AndCombinator}";
    
    public static readonly Parser<ExpressionCombinator> _binaryOperator = Parse
        .String(ODataExpressionCombinator.OrCombinator)
        .Return(ExpressionCombinator.Or)
        .Or(Parse.String(ODataExpressionCombinator.AndCombinator).Return(ExpressionCombinator.And));

    public static TreeNode? next = null;
    public static RootNode? rootNode = null;


    public static  Parser<ExpressionCombinator> And =>
        Parse.String(ODataExpressionCombinator.AndCombinator).Return(ExpressionCombinator.And);

    public static  Parser<ExpressionCombinator> Or =>
        Parse.String(ODataExpressionCombinator.OrCombinator).Return(ExpressionCombinator.Or);
    
    public static Parser<TreeNode> RootNodeParsed =>
        from expressionNodeString in ExpressionNode
        select CreateRootNodeExpression(
            expressionNodeString,
            Parse.AnyChar.Many().Token().ToString(),
            OdataFilterOption.DollarFilter);

    public static Parser<TreeNode> QueryParse =>
        Parse.ChainOperator(
            And.Or(Or),
            ExpressionNode.Or(ExpressionNode),
            CreateLeaft);
    
    public static Parser<OperatorType> OperatorEnum =>
        Parse.String(ODataExpressionOperators.EqualsOperator)
            .Return(OperatorType.EqualsOperator)
            .Or(Parse.IgnoreCase(ODataExpressionOperators.GreaterThenOperator).Return(OperatorType.GreaterThenOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.GreaterEqualsOperator).Return(OperatorType.GreaterEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.LessThenOperator).Return(OperatorType.LessThenOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.LessEqualsOperator).Return(OperatorType.LessEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.NotEqualsOperator).Return(OperatorType.NotEqualsOperator))
            .Or(Parse.IgnoreCase(ODataExpressionOperators.EqualsOperator).Return(OperatorType.EqualsOperator));
    
    public static Parser<string> InnerValue => InnerQuotedValue.Or(InnerNotQuotedValue);

    public static Parser<string> InnerQuotedValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from openQuotedValue in Parse.Char(QuotedCharacter.SingleQuotedCharacter)
        from innerValue in Parse.CharExcept(QuotedCharacter.SingleQuotedCharacter)
            .Many()
            .Text()
            .Token()
            .Named("QuoatedValue(right-side)")
        from closeQuotedValue in Parse.Char(QuotedCharacter.SingleQuotedCharacter)
        select innerValue;

    public static Parser<string> InnerNotQuotedValue =>
        from dataType in Parse.Regex("[A-Za-z]*").Text().Token()
        from innerValueWithoutQuotation in Parse.Number.Text().Token().Named("InnerNotQuotedValue(right-side)")
        select innerValueWithoutQuotation;

    public static Parser<string> LeftHandSide =>
        Parse.Regex("[A-Za-z]+").Text().Token().Named("LeftHandSideExpression");

    public static Parser<TreeNode> ExpressionNode =>
        from leftHandSide in LeftHandSide
        from operatorExpression in OperatorEnum
        from rightHandSideValue in InnerValue
        select CreateExpressionNode(leftHandSide, rightHandSideValue, operatorExpression);

  /*  public static Parser<TreeNode> BinaryExpressionNode =>
        from leftHandSide in ExpressionNode.Token()
        from @operator in _binaryOperator.Token()
        from rightSide in ExpressionNode.Token()
        select CreateBinaryNode(null, leftHandSide, rightSide, @operator);*/

    public static TreeNode CreateExpressionNode(string leftHandSide, string rightHandType, OperatorType operatorType)
    {
        return new ExpressionNode(leftHandSide, rightHandType, operatorType);
    }

    public static TreeNode CreateBinaryNode(string query, TreeNode left, TreeNode right, ExpressionCombinator @operator)
    {

        return new BinaryExpression(left, right, @operator);
    }
    
    public static TreeNode CreateLeaft (ExpressionCombinator op, TreeNode l, TreeNode right)
    {
        if (rootNode == null)
        {
            rootNode = new RootNode(OdataFilterOption.DollarFilter, "test");
            rootNode.LeftChild = new BinaryExpression(l, rootNode, op);
            next = ((BinaryExpression)rootNode.LeftChild).RightChild;

            return rootNode;
        }
        
        var temp = new BinaryExpression(l, right, op);
        next = temp;
        next = ((BinaryExpression)next).RightChild;
        
        return rootNode;
    }


    public static TreeNode CreateRootNodeExpression(TreeNode treeNode, string query, OdataFilterOption option)
    {
        var root = new RootNode(option, query)
        {
            LeftChild = treeNode
        };

        return root;
    }
}

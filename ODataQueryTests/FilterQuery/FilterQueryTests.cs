using FluentAssertions;
using ODataQueryTests.Extensions;
using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataQueryTests.FilterQuery;

public class FilterQueryTests
{
   
    [Fact]
    public void FilterStringObjectRootTest()
    {
        const string queryString = "$filter=Date ge datetime’2022-01-01T00:00:00 and Name eq 'Andreas'";
        TreeNode? rootNode = FilterQueryGrammar.RootNodeParsed.Parse(queryString);
        var queryResult = "Date ge datetime’2022-01-01T00:00:00 and Name eq Andreas'";

        TreeNode rootNodeToCompare = new RootNode(OdataFilterOption.DollarFilter, queryResult);

        rootNode.Should().BeEquivalentTo(rootNodeToCompare, opt => opt.Excluding(o => o.Id).RespectingRuntimeTypes());

    }

    [Theory]
    [InlineData(ODataExpressionOperators.LessEqualsOperator)]
    [InlineData(ODataExpressionOperators.LessThenOperator)]
    [InlineData(ODataExpressionOperators.EqualsOperator)]
    [InlineData(ODataExpressionOperators.NotEqualsOperator)]
    [InlineData(ODataExpressionOperators.GreaterThenOperator)]
    [InlineData(ODataExpressionOperators.GreaterEqualsOperator)]
    public void FilterQueryCreateEqualsExpressionTests(string @operator)
    {
        var queryString = $"Date {@operator} datetime′2022-01-01T00:00:00′";
        TreeNode? rootNode = FilterQueryGrammar.ExpressionNode.Parse(queryString);
        var rootNodeToCompare = new ExpressionNode("Date", "2022-01-01T00:00:00", @operator.GetOperatorType());
        rootNode.Should().BeEquivalentTo(rootNodeToCompare, opt => opt.RespectingRuntimeTypes().Excluding(o => o.Id));
    }

    [Theory]
    [InlineData("Date eq datetime′2022-01-01T00:00:00′", "2022-01-01T00:00:00")]
    [InlineData("Id eq 123", "123")]
    public void FilterQueryParseInnerValue(string query, string rightHandSide)
    {
        var parser = FilterQueryGrammar.ExpressionNode.Parse(query);
        ((ExpressionNode)parser).RightSideExpression.Should().BeEquivalentTo(rightHandSide);
    }

  /*  [Theory]
    [InlineData(ODataExpressionCombinator.OrCombinator)]
    [InlineData(ODataExpressionCombinator.AndCombinator)]
    public void FilterQueryCreateBinaryExpressionNode(string expressionCombinator)
    {
        var queryString = $"Date lt datetime′2022-01-01T00:00:00′ {expressionCombinator} Name ne ′Andreas′";
        TreeNode? binaryExpression = FilterQueryGrammar.BinaryExpressionNode.Parse(queryString);

        var leftQuerySide = new ExpressionNode("Date", "2022-01-01T00:00:00", OperatorType.LessThenOperator);
        var rightQuerySide = new ExpressionNode("Name", "Andreas", OperatorType.NotEqualsOperator);

        var binaryExpressionTree = new BinaryExpression(
            leftQuerySide,
            rightQuerySide,
            expressionCombinator.GetExpressionCombinator());

        binaryExpression.Should()
            .BeEquivalentTo(
                binaryExpressionTree,
                opt => opt.Excluding(o => o.Id)
                    .Excluding(o => o.LeftChild.Id)
                    .Excluding(o => o.RightChild.Id)
                    .RespectingRuntimeTypes());
    }*/

    [Fact]
    public void FilterRootNodeWithOneExpression()
    {
        var queryString = "$filter=Date eq datetime′2022-01-01T00:00:00′";
        var queryOption = "filter";
        
        var resultQueryString = new OdataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        
        var treeNode = FilterQueryGrammar.QueryParse.Parse(resultQueryString) ;
    }
    
    [Fact]
    public void FilterRootNodeTree()
    {
        var queryString = "$filter=Date eq datetime′2022-01-01T00:00:00′ and name eq 12";
        var queryOption = "filter";
        
        var resultQueryString = new OdataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        
        var treeNode = FilterQueryGrammar.QueryParse.Parse(resultQueryString) ;
    }
    
}

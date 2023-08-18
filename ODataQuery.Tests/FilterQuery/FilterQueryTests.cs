using System.Text.RegularExpressions;
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

        TreeNode rootNodeToCompare = new RootNode(ODataFilterOption.DollarFilter, queryResult);

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

    [Fact]
    public void FilterRootNodeWithOneExpression()
    {
        var queryString = "$filter=Date eq datetime′2022-01-01T00:00:00′";
        var queryOption = "filter";
        
        var resultQueryString = new ODataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        
        var treeNode = FilterQueryGrammar.QueryParse.Parse(resultQueryString) ;
    }
    
    [Fact]
    public void FilterRootNodeTree()
    {
        var queryString = "$filter=Date ge datetime′2022-01-01T00:00:00′ and Name eq ′Andreas′ or Name eq ′Stefan′";
        var queryOption = "filter";
        
        var resultQueryString = new ODataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        
        var treeNode = FilterQueryGrammar.QueryParse.Parse(resultQueryString) ;
    }

    
    [Fact]
    public void CheckIfOrOrAndContainsTheString_should_not_match()
    {
        var queryString = "Date ge 123 or Date eq 456";
        var noteparse = FilterQueryGrammar.CheckForOperator.Parse(queryString);
        
    }
}

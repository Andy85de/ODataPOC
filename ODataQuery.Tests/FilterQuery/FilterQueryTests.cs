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
        const string queryString = "$filter=Date ge datetime'2022-01-01T00:00:00' and Name eq 'Andreas'";
        TreeNode? rootNode = FilterQueryGrammar.RootNodeParsed.Parse(queryString);
        var queryResult = "Date ge datetime'2022-01-01T00:00:00' and Name eq 'Andreas'";

        TreeNode rootNodeToCompare = new RootNode(ODataFilterOption.DollarFilter, queryResult);

        rootNode.Should().BeEquivalentTo(rootNodeToCompare, opt => opt.Excluding(o => o.Id).RespectingRuntimeTypes());
    }

    [Theory]
    [InlineData(ODataLikeExpressionOperators.LessEqualsOperator)]
    [InlineData(ODataLikeExpressionOperators.LessThenOperator)]
    [InlineData(ODataLikeExpressionOperators.EqualsOperator)]
    [InlineData(ODataLikeExpressionOperators.NotEqualsOperator)]
    [InlineData(ODataLikeExpressionOperators.GreaterThenOperator)]
    [InlineData(ODataLikeExpressionOperators.GreaterEqualsOperator)]
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

        string? resultQueryString =
            new ODataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);

        FilterQueryGrammar.SetQueryString(queryString);
        var treeNodeResult = FilterQueryGrammar.QueryFilterParser.Parse(resultQueryString);

        var expressionNode = new ExpressionNode("Date", "2022-01-01T00:00:00", OperatorType.EqualsOperator);

        var treeNodeToCompare = new RootNode(ODataFilterOption.DollarFilter, queryString)
        {
            LeftChild = expressionNode
        };

        treeNodeResult.Should()
            .BeEquivalentTo(
                treeNodeToCompare,
                opt => opt.Excluding(t => t.Id).Excluding(t => t.LeftChild.Id).RespectingRuntimeTypes());
    }
    
    [Fact]
    public void FilterRootNodeTree()
    {
        var queryString = "$filter=Date ge datetime′2022-01-01T00:00:00′ and Name eq ′Andreas′ or Name eq ′Stefan′ or Name ne ′Stefan′";
        var queryOption = "filter";
        
        var resultQueryString = new ODataQueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        FilterQueryGrammar.SetQueryString(queryString);
        var treeNode = FilterQueryGrammar.QueryFilterParser.Parse(resultQueryString) ;
    }

    
    [Fact]
    public void CheckIfOrOrAndContainsTheString_should_not_match()
    {
        var queryString = "Date ge 123 or Date eq 456";
        FilterQueryGrammar.SetQueryString(queryString);
        var noteparse = FilterQueryGrammar.QueryBinaryParser.Parse(queryString);
    }

    [Theory]
    [InlineData("Customer/Bill/Id", "Customer.Bill.Id")]
    [InlineData("Customer/Bill/Amount/Dollar/Prize","Customer.Bill.Amount.Dollar.Prize")]
    [InlineData("Customer/Bill", "Customer.Bill")]
    public void LeftHandSideNestedClause(string query, string result)
    {
        var queryParser = FilterQueryGrammar.LeftHandSideNested;

        var resultQuery = queryParser.Parse(query);

        resultQuery.Should().BeEquivalentTo(result);
        
    }
    
}

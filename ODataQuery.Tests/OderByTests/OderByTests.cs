using System.Linq.Expressions;
using FluentAssertions;
using ODataWithSprache;
using ODataWithSprache.Extensions;
using ODataWithSprache.Grammar;
using ODataWithSprache.Models;
using Remotion.Linq.Clauses;
using Sprache;

namespace ODataQueryTests.OderByTests;

public class OderByTests
{
    public static List<object[]> _testArgumentsForOnePropertyData = new List<object[]>
    {
        new object[] { "BaseRate asc", new SortedProperty("BaseRate", SortDirection.Ascending) },
        new object[] { "BaseRate", new SortedProperty("BaseRate", SortDirection.Descending) },
        new object[] { "CreatedAt desc  ", new SortedProperty("CreatedAt", SortDirection.Descending) }
    };


    public static List<object[]> _testArgumentsForVariousPropertyData = new List<object[]>()
    {
        new object[]
        {
            "BaseRate, CreatedAt asc, SortedId",
            new List<SortedProperty>
            {
                new SortedProperty("BaseRate", SortDirection.Descending),
                new SortedProperty("CreatedAt", SortDirection.Ascending),
                new SortedProperty("SortedId", SortDirection.Descending)
            }
        },
        
        new object[]
        {
            "BaseRate,CreatedAt,SortedId",
            new List<SortedProperty>
            {
                new SortedProperty("BaseRate", SortDirection.Descending),
                new SortedProperty("CreatedAt", SortDirection.Descending),
                new SortedProperty("SortedId", SortDirection.Descending)
            }
        },
        new object[]
        {
            "BaseRate DESC,CreatedAt asc,SortedId DESC, Items ASC",
            new List<SortedProperty>
            {
                new SortedProperty("BaseRate", SortDirection.Descending),
                new SortedProperty("CreatedAt", SortDirection.Ascending),
                new SortedProperty("SortedId", SortDirection.Descending),
                new SortedProperty("Items", SortDirection.Ascending)
            }
        },
        new object[]
        {
            "BaseRate DESC,CreatedAt asc,SortedId DESC, Items ASC, Bill, UpdateAt asc",
            new List<SortedProperty>
            {
                new SortedProperty("BaseRate", SortDirection.Descending),
                new SortedProperty("CreatedAt", SortDirection.Ascending),
                new SortedProperty("SortedId", SortDirection.Descending),
                new SortedProperty("Items", SortDirection.Ascending),
                new SortedProperty("Bill",SortDirection.Descending),
                new SortedProperty("UpdateatAt", SortDirection.Ascending)
            }
        }
    };

    public static List<object[]> _testForOneItemShouldThrowException = new List<object[]>
    {
        new object[]
            {
                "BaseRate, asc,",
                new SortedProperty("BaseRate", SortDirection.Ascending)
            }
        

    };

    [Theory]
    [MemberData(nameof(_testArgumentsForOnePropertyData))]
    public void OrderByOneProperty(string query, SortedProperty sortedProperty)
    {
        var oderByParser = new OderByQueryGrammar();

        List<SortedProperty>? result = oderByParser.OrderByListQuery.Parse(query);

        Assert.True(result.Count == 1);

        result.First().Should().BeEquivalentTo(sortedProperty);
    }

    [Theory]
    [MemberData(nameof(_testArgumentsForVariousPropertyData))]
    public void OderByWithMoreProperties(string query, List<SortedProperty> resultSet)
    {
        var result = new OderByQueryGrammar().OrderByListQuery.Parse(query);
        result.Should().BeEquivalentTo(resultSet);
    }

    [Theory]
    [MemberData(nameof(_testForOneItemShouldThrowException))]
    public void order_by_one_property_should_fail(string query, SortedProperty resultProperty)
    {
        var parser = new OderByQueryGrammar().OrderByListQuery;

        Assert.Throws<Exception>(() => parser.Parse(query));
    }

    [Theory]
    [InlineData(
        "BaseRate, CreatedAt asc, SortedId",
        "data",
        "ORDER BY data ->> 'BaseRate desc', data ->> 'CreatedAt asc', data ->> 'SortedId desc'")]
    public void order_by_check_jsonb_query_should_work(string oderByQuery, string JsonbVariable, string resultQuery)
    {
        var oderByParser = new OderByQueryGrammar().OrderByListQuery;
        var resultProperties = oderByParser.Parse(oderByQuery);

        var createdResultQuery = resultProperties.GetOrderByJsonBQuery(JsonbVariable);

        createdResultQuery.Should().BeEquivalentTo(resultQuery);
    }
    
    
}

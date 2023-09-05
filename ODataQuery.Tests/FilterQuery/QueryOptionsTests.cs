using FluentAssertions;
using ODataWithSprache.Grammar;
using Sprache;

namespace ODataQueryTests.FilterQuery;

public class QueryOptionsTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetFilterStringWholeFilterStringWithAmpersand(bool isUpperCase)
    {
        var queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′&$orderby=name,desc";
        var queryResult = "Date ge datetime’2021-01-01T00:00:00′";
        var queryOption = "filter";

        if (isUpperCase)
        {
            queryString = queryString.ToUpper();
            queryResult = queryResult.ToUpper();
        }

        string? filterQuery = new QueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);

        Assert.Equal(filterQuery, queryResult);
    }

    [Fact]
    public void GetFilterStringWholeFilterStringWithoutAmpersand()
    {
        const string queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′$orderby=name,desc";
        const string queryResult = "Date ge datetime’2021-01-01T00:00:00′";
        const string queryOption = "filter";

        string? filterQuery = new QueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);

        Assert.Equal(filterQuery, queryResult);
    }

    [Fact]
    public void GetFilterStringWholeFilterStringWithoutEndingCharacters()
    {
        const string queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′";
        const string queryResult = "Date ge datetime’2021-01-01T00:00:00′";
        const string queryOption = "filter";
        
        string? filterQuery = new QueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);

        Assert.Equal(filterQuery, queryResult);
    }

    [Theory]
    [InlineData("http://ServiceRoot/Movies?$select=Id,Name,Classification,RunningTime&$filter=contains(Name, 'li')&$orderby=Name desc","Name desc", "orderBy")]
    [InlineData("http://ServiceRoot/Movies?$select=Id,Name,Classification,RunningTime&$filter=contains(Name, 'li')&$orderby=Name desc","Id,Name,Classification,RunningTime", "select")]
    [InlineData("http://ServiceRoot/Movies?$select=Id,Name,Classification,RunningTime&$filter=contains(Name, 'li')&$orderby=Name desc","contains(Name, 'li')", "filter")]
    public void GetOrderByStringOutOfLongQuery(string queryString, string resultCompareString, string filterOption)
    {
        var parsedQuery = new QueryOptionsParser(filterOption).PartedQueryForSpecialOption.Parse(queryString);
        resultCompareString.Should().BeEquivalentTo(parsedQuery);
    }

}

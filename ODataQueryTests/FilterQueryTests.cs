using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;
using Sprache;

namespace ODataQueryTests
{
    public class FilterQueryTests
    {
        [Theory, InlineData(true), InlineData(false)]
        public void FilterStringWholeFilterStringWithAmperSand(bool isUpperCase)
        {
            string queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′&$orderby=name,desc";
            string queryResult="Date ge datetime’2021-01-01T00:00:00′";

            if (isUpperCase)
            {
                queryString = queryString.ToUpper();
                queryResult=queryResult.ToUpper();
            }
            
            var filterQuery = FilterQueryGrammar.WholeFilterQuery.Parse(queryString);
            
            Assert.Equal(filterQuery, queryResult);
        }
        
        [Fact]
        public void FilterStringWholeFilterStringWithoutAmperSand()
        {
            const string queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′$orderby=name,desc";
            const string queryResult = "Date ge datetime’2021-01-01T00:00:00′";

            var filterQuery = FilterQueryGrammar.WholeFilterQuery.Parse(queryString);
            
            Assert.Equal(filterQuery, queryResult);
        }
        
        [Fact]
        public void FilterStringWholeFilterStringWithoutEndingCharacters()
        {
            const string queryString = "$filter=Date ge datetime’2021-01-01T00:00:00′";
            const string queryResult = "Date ge datetime’2021-01-01T00:00:00′";
            var filterQuery = FilterQueryGrammar.WholeFilterQuery.Parse(queryString);
            
            Assert.Equal(filterQuery, queryResult);
        }

        [Fact]
        public void FilterStringWithExpressionCombinator()
        {
            const string queryString = "$filter=Date ge datetime’2022-01-01T00:00:00 and Name eq 'Andreas'";
        }

        [Fact]
        public void FilterStringObjectRootTest()
        {
            const string queryString = "$filter=Date ge datetime’2022-01-01T00:00:00 and Name eq 'Andreas'";
            var rootNode = FilterQueryGrammar.Query.Parse(queryString);
            var queryResult = "Date ge datetime’2022-01-01T00:00:00 and Name eq 'Andreas'";
            
            Assert.Equal(queryResult, ((RootNode)rootNode).OriginalString);
            Assert.Equal(OdataFilterOption.DollarFilter, ((RootNode)rootNode)._operatorType);
        }
        
        [Theory]
        [InlineData("lt")]
        public void FilterQueryCreateEqualsExpression(string @operator)
        {
            string queryString = $"Date {@operator} datetime′2022-01-01T00:00:00′";
            var rootNode = FilterQueryGrammar.ExpressionNode.Parse(queryString);
            
        }
    }
}

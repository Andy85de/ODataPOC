﻿using FluentAssertions;
using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;
using ODataWithSprache.Visitors;
using Sprache;

namespace ODataQueryTests.FilterQuery;

public class VisitorTests
{
    public static List<object[]> _filterTestArguments = new List<object[]>
    {
        new object[]
        {
            "$filter=Date/Time ge datetime'2021-01-01T00:00:00'", "WHERE Date.Time >= '31/12/2020 23:00:00'"
        },
        new object[] { "$filter=Name eq 'Andreas'", "WHERE Name = 'Andreas'" },
        new object[] { "$filter=City ne 'Orlando'", "WHERE City != 'Orlando'" },
        new object[] { "$filter=Name eq 'Orange'", "WHERE Name = 'Orange'" },
        new object[] { "$filter=Amount le 123", "WHERE Amount <= 123" },
        new object[] { "$filter=Name eq 'Andreas' and Amount lt 123", "WHERE Name = 'Andreas' AND Amount < 123" },
        new object[] { "$filter=Amount lt 123 and Name eq 'Andreas'", "WHERE Amount < 123 AND Name = 'Andreas'" },
        new object[]
        {
            "$filter=Amount lt 123 and Name eq 'Andreas' or Date/Time ge datetime'2021-01-01T00:00:00'",
            "WHERE Amount < 123 AND Name = 'Andreas' OR Date.Time >= '31/12/2020 23:00:00'"
        },

        new object[]
        {
            "$filter=Amount lt 123 and Name eq 'Andreas' or Date/Time ge datetime'2021-01-01T00:00:00' and Wallet/Money/Id eq '8D2A49E4-02FE-4A77-9D91-EB8543310381'",
            "WHERE Amount < 123 AND Name = 'Andreas' OR Date.Time >= '31/12/2020 23:00:00' AND Wallet.Money.Id = '8D2A49E4-02FE-4A77-9D91-EB8543310381'"
        },

        new object[]
        {
            "$filter= Time eq 'PM' or Name eq 'Abdrachman' or Amount/Id eq 15789'",
            "WHERE Time = 'PM' OR Name = 'Abdrachman' OR Amount.Id = 15789"
        }
    };
    
    [Theory]
    [MemberData(nameof(_filterTestArguments))]   
    public void Create_easy_Sql_query_Should_work(string filterQuery, string resultSqlQuery)
    {
        string? optionParserFilter =
            new ODataQueryOptionsParser("filter").PartedQueryForSpecialOption.Parse(filterQuery);

        FilterQueryGrammar.SetQueryString(optionParserFilter);
        TreeNode? nodeTree = FilterQueryGrammar.QueryFilterParser.Parse(optionParserFilter);

        var sqlVisitor = new TreeNodeFilterVisitor();
        string sqlString = sqlVisitor.Visit((RootNode)nodeTree);

        FilterQueryGrammar.Clear();

        resultSqlQuery.Should().BeEquivalentTo(sqlString);
    }
}

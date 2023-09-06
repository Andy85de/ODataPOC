using System.Linq.Expressions;
using JasperFx.Core;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using ODataWithSprache.Extensions;
using ODataWithSprache.Grammar;
using ODataWithSprache.Implementation;
using ODataWithSprache.Models;
using ODataWithSprache.TreeStructure;
using ODataWithSprache.Visitors;
using Sprache;

namespace ODataWithSprache.Controller;

[Controller]
[Route("api")]
public class RequestController: ControllerBase
{
    private ILogger<RequestController> _Logger;
    private readonly IDocumentSession _DocumentSession;
    
    public RequestController(ILogger<RequestController> logger, IDocumentStore documentStorage)
    {
        _Logger = logger;
        _DocumentSession = documentStorage.LightweightSession();
    }

    [HttpGet]
    public async Task<IActionResult> GetRequestForMarten([FromQuery] FilterQuery query)
    {
       var request = _DocumentSession.Query<UserSettingObjectMarten>()
            .Where(r => true)
            .OrderBy("")
            .ToPagedList(1,1);
       
        return Ok(request);
    }
    
    [HttpGet("TestExpression")]
    public async Task<IActionResult> GetExpressionQuery([FromQuery] FilterQuery query)
    {
        var queryString = $"$filter={query.Filter}";
        var orderByQuery = $"{query.OrderByQuery}";
        
        var queryOption = "filter";

        string? resultQueryString =
            new QueryOptionsParser(queryOption).PartedQueryForSpecialOption.Parse(queryString);
        FilterQueryGrammar.SetQueryString(queryString);
        
        var orderByObjects = new OderByQueryGrammar().OrderByListQuery.Parse(orderByQuery);
        
        var oderByString = orderByObjects
            .Select(p => $"{p.PropertyName} {(p.Sorted == SortDirection.Ascending ? "asc" : "desc")}")
            .ToArray();
        
        TreeNode? treeNodeResult = FilterQueryGrammar.QueryFilterParser.Parse(resultQueryString);
        
        var filterExpression = new TreeNodeExpressionVisitor().Visit<UserSettingObjectMarten>(treeNodeResult);

        IPagedList<UserSettingObjectMarten> result = _DocumentSession.Query<UserSettingObjectMarten>()
            .Where(filterExpression)
            .OrderBy(orderByQuery)
            .ToPagedList(query.Offset, query.Limit);

        return Ok(result);
    }
    
    

    [HttpPost]
    public async Task<IActionResult> CreateRequestingData()
    {
        IReadOnlyList<UserSettingObjectMarten> entities =
            await _DocumentSession.Query<UserSettingObjectMarten>().ToListAsync();

        if (entities.Count > 0)
        {
            return Accepted("The Data were already create");
        }

        _DocumentSession.Insert<UserSettingObjectMarten>(_testObjectToStore);

        await _DocumentSession.SaveChangesAsync();

        return Ok("Sample date has been created.");
    }

    public static List<UserSettingObjectMarten> _testObjectToStore = new List<UserSettingObjectMarten>
    {

        new UserSettingObjectMarten(
            DateTime.Now.AddMonths(-1),
            1,
            "5",
            DateTime.Now.AddDays(-1),
            "First Month",
            100),
        new UserSettingObjectMarten(
            DateTime.Now.AddMonths(-2),
            2,
            "4",
            DateTime.Now.AddDays(-2),
            "Second Month",
            200),
        new UserSettingObjectMarten(
            DateTime.Now.AddMonths(-3),
            3,
            "3",
            DateTime.Now.AddDays(-3),
            "Third Month",
            300),
        new UserSettingObjectMarten(
            DateTime.Now.AddMonths(-4),
            4,
            "2",
            DateTime.Now.AddDays(-4),
            "Forth Month",
            400),
        new UserSettingObjectMarten(
            DateTime.Now.AddMonths(-4),
            5,
            "1",
            DateTime.Now.AddDays(-5),
            "Fifth Month",
            500)
    };
}

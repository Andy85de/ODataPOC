using Microsoft.AspNetCore.Mvc;
using ODataWithSprache.Abstraction;

namespace ODataWithSprache.Implementation;

public class FilterQuery : FilterBase
{
    [FromQuery(Name = "$filter")]
    public override string? Filter { get; set; } 

    [FromQuery(Name = "$orderBy")]
    public override string? OrderByQuery { get; set; }
}

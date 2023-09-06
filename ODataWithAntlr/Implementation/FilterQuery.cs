using Microsoft.AspNetCore.Mvc;
using ODataWithSprache.Abstraction;

namespace ODataWithSprache.Implementation;

public class FilterQuery : FilterBase
{
    [FromQuery(Name = "$filter")]
    public override string? Filter { get; set; } 

    [FromQuery(Name = "$orderBy")]
    public override string? OrderByQuery { get; set; }

    public override int Offset { get; set; } = 1;

    public override int Limit { get; set; } = 50;
}

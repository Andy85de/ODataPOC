using System.Linq.Expressions;
using ODataWithSprache.Grammar;

namespace ODataWithSprache.Models;

public class SortedPropertyLamdaExpression
{
    public string Property { get; set; }

    public SortDirection Direction { get; set; }

    public Expression LamdaExpression ;
}


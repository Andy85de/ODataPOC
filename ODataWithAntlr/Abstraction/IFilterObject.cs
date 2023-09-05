namespace ODataWithSprache.Abstraction;

public interface IFilterObject
{
    /// <summary>
    ///     The filter query that is used to filter the result objects.
    /// </summary>
    public string Filter { set; get; }

    /// <summary>
    ///     The oder by query that is used to order the result objects for one
    ///     or more specific Property.
    /// </summary>
    public string OrderByQuery { set; get; }

    /// <summary>
    ///     The number of items to skip before starting to collect the result set.
    /// </summary>
    int Offset { set; get; }

    /// <summary>
    ///     The number of items to return.
    /// </summary>
    int Limit { set; get; }
}

namespace ODataWithSprache.Abstraction;

public abstract class FilterBase : IFilterObject
{
    /// <inheritdoc />
    public virtual string? Filter { get; set; } = string.Empty;

    /// <inheritdoc />
    public virtual string? OrderByQuery { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Offset { get; set; } = 0;

    /// <inheritdoc />
    public int Limit { get; set; } = 50;
}

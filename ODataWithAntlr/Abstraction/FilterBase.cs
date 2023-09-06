namespace ODataWithSprache.Abstraction;

public abstract class FilterBase : IFilterObject
{
    /// <inheritdoc />
    public virtual string? Filter { get; set; } = string.Empty;

    /// <inheritdoc />
    public virtual string? OrderByQuery { get; set; } = string.Empty;

    /// <inheritdoc />
    public virtual int Offset { get; set; } = 1;

    /// <inheritdoc />
    public virtual int Limit { get; set; } = 50;
}

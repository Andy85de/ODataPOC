namespace ODataWithSprache.Grammar;

/// <summary>
///  The combinator combines two expression with an binary operator.
///  The supported combinators are the "and" and "or"-combinators.
/// </summary>
public enum ExpressionCombinator
{
    /// <summary>
    /// 
    /// </summary>
    None,
    
    /// <summary>
    /// 
    /// </summary>
    And,
    
    /// <summary>
    /// The or combinator that connects two expressions.
    /// </summary>
    Or
}

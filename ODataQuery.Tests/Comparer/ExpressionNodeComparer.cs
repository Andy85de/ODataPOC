using System.Collections;
using ODataWithSprache.TreeStructure;

namespace ODataQueryTests.Comparer;

public class ExpressionNodeComparer : IEqualityComparer<ExpressionNode>
{
    public bool Equals(ExpressionNode? x, ExpressionNode? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return
            x.LeftSideExpression == y.LeftSideExpression
            && x.Operator == y.Operator
            && x.RightSideExpression == y.RightSideExpression;
    }

    public int GetHashCode(ExpressionNode obj)
    {
        return HashCode.Combine(
            obj.Id,
            obj.Root,
            obj.Parent,
            obj.LeftSideExpression,
            (int)obj.Operator,
            obj.RightSideExpression,
            obj.ValueType);
    }
} 
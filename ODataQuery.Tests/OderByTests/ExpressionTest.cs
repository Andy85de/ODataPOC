using System.Linq.Expressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ODataQueryTests.OderByTests;

public class ExpressionTest
{
    [Fact]
    public void TryTestOderBy()
    {

        var list = new List<UserSettingObject>
        {
            new UserSettingObject
            {
                Id = "3",
                CreatedAt = DateTime.Now.AddMonths(-1),
                SectionName = "Favorites",
                CreatedAtMonth = $"{DateTime.Now.AddMonths(-1):M}"
            },
            new UserSettingObject
            {
                Id = "2",
                CreatedAt = DateTime.Now.AddMonths(-2),
                SectionName = "Favorites",
                CreatedAtMonth =$"{DateTime.Now.AddMonths(-2):M}"
                
            },
            new UserSettingObject
            {
                Id = "1",
                CreatedAt = DateTime.Now.AddMonths(-3),
                SectionName = $"Favorites",
                CreatedAtMonth = $"{DateTime.Now.AddMonths(-3):M}"
            }
        };
        
        //      Expression<Func<UserSettingObject, DateTime>> t = m => m.CreatedAt;
        //      Func<UserSettingObject, DateTime> func = t.Compile();

        var parameter = Expression.Parameter(typeof(UserSettingObject), "m");

        /*var p = Expression.Lambda(
            typeof(Func<,>).MakeGenericType(typeof(UserSettingObject), typeof(DateTime)),
            Expression.Property(parameter, typeof(UserSettingObject).GetProperty("CreatedAt")),
            parameter);*/

        var p = Expression.Lambda(
            typeof(Func<,>).MakeGenericType(typeof(UserSettingObject), typeof(DateTime)),
            Expression.Property(parameter, typeof(UserSettingObject).GetProperty("CreatedAt")),
            parameter);
        
        
        
        var sortedList = list.OrderBy(
            (p as Expression<Func<UserSettingObject, DateTime>>).Compile());
        
    }

    public class ExpressionLambdas<TResult>
    {

        Expression? MakeExpressionLamdba<TResult, TData>(string property, Type propertyType)
        {
            var parameter = Expression.Parameter(typeof(TResult), "m");
            
            var lamda =Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(TResult), propertyType),
                Expression.Property(parameter, typeof(TResult).GetProperty(property)),
                parameter);

            return lamda as Expression<Func<TResult, TData>>;
        }
    }
    
}

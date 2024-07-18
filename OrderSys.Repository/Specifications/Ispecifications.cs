using OrderSys.Core.Entities;
using System.Linq.Expressions;

namespace OrderSys.Core.Specifications
{
    public interface Ispecifications<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; set; }

    }
}

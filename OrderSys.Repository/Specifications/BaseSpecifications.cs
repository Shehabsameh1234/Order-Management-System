
using OrderSys.Core.Entities;
using System.Linq.Expressions;


namespace OrderSys.Core.Specifications
{
    public class BaseSpecifications<T> : Ispecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null!;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        
        public BaseSpecifications()
        {
            
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

    }
}

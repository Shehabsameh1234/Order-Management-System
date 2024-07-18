using Microsoft.EntityFrameworkCore;
using OrderSys.Core.Entities;
using OrderSys.Core.Specifications;


namespace OrderSys.Repository
{
    internal class SpecificationEvaluater <T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuey, Ispecifications<T> spec)
        {
            var query = inputQuey;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            

            return query;

        }
    }
}

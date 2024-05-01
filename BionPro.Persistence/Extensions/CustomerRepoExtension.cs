using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class CustomerRepoExtension
{
    public static IQueryable<CustomerMD> Sort(this IQueryable<CustomerMD> customers, string? orderByQueryString)
    {
        //The example format of orderByQueryString is "name, age desc"
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return customers.OrderBy(e => e.CustomerName); //If there is no order by, then sort by customer name

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<CustomerMD>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return customers.OrderBy(e => e.CustomerName);

        return customers.OrderBy(orderQuery);
    }
}

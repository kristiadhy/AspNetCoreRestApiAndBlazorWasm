using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class SupplierRepoExtension
{
    public static IQueryable<SupplierModel> Sort(this IQueryable<SupplierModel> suppliers, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return suppliers.OrderBy(e => e.SupplierName);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<SupplierModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return suppliers.OrderBy(e => e.SupplierName);

        return suppliers.OrderBy(orderQuery);
    }
}

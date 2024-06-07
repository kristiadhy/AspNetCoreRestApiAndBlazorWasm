using Domain.Entities;

namespace Persistence.Extensions;
public static class SupplierQueryExtension
{
    public static IQueryable<SupplierModel> SearchByName(this IQueryable<SupplierModel> suppliers, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return suppliers;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return suppliers.Where(e => e.SupplierName!.ToLower().Contains(lowerCaseTerm));
    }
}

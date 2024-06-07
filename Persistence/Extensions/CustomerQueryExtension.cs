using Domain.Entities;

namespace Persistence.Extensions;
public static class CustomerQueryExtensions
{
    //public static IQueryable<CustomerMD> FilterByAge(this IQueryable<CustomerMD> customers, uint minAge, uint maxAge) 
    //    => customers.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<CustomerModel> SearchByName(this IQueryable<CustomerModel> customers, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return customers;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return customers.Where(e => e.CustomerName!.ToLower().Contains(lowerCaseTerm));
    }
}

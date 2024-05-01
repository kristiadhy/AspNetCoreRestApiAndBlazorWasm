using Domain.Entities;

namespace Persistence.Extensions;
public static class CustomerQueryExtensions
{
    //public static IQueryable<CustomerMD> FilterByAge(this IQueryable<CustomerMD> employees, uint minAge, uint maxAge) 
    //    => employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<CustomerMD> SearchByName(this IQueryable<CustomerMD> employees, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return employees.Where(e => e.CustomerName!.ToLower().Contains(lowerCaseTerm));
    }
}

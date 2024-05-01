using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<CustomerMD> Customer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //It will take all configurations from IEntityTypeConfiguration interface and run it.
        //Please refer to this link : https://learn.microsoft.com/en-us/ef/core/modeling/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
    }
}
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using System.Reflection.Metadata;

namespace Persistence.Context;

public class AppDBContext : IdentityDbContext<UserMD>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
    {

    }

    public DbSet<CustomerMD> Customer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //It will take all configurations from IEntityTypeConfiguration interface and run it.
        //Please refer to this link : https://learn.microsoft.com/en-us/ef/core/modeling/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);

        //new CustomerConfiguration().Configure(modelBuilder.Entity<CustomerMD>());
    }
}
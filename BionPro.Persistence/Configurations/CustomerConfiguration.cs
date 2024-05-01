using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<CustomerMD>
{
    public void Configure(EntityTypeBuilder<CustomerMD> builder)
    {
        builder.ToTable("M01CUSTOMERS");

        builder.HasKey(c => c.CustomerID);

        builder.Property(c => c.CustomerID).ValueGeneratedOnAdd();
        builder.Property(c => c.CustomerName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.ContactPerson).HasMaxLength(100);
        builder.Property(c => c.CPPhone).HasMaxLength(50);
    }
}

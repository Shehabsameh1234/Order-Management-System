using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSys.Core.Entities;

namespace OrderSys.Repository.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.Property(p => p.TotalAmount)
                   .HasColumnType("decimal (18,2)");

            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId);
    
        }
    }
}

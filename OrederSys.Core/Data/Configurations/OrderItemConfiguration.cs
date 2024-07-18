using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSys.Core.Entities;


namespace OrderSys.Repository.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(oi => oi.UnitPrice)
                   .HasColumnType("decimal (18,2)");

            builder.Property(oi => oi.Discount)
                   .HasColumnType("decimal (18,2)");

        }
    }
}

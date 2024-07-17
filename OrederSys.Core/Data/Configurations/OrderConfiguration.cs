using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;

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
                   .HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.PaymentMethod).HasConversion(
           (oMethod) => oMethod.ToString(),
           (oMethod) => (PaymentMethods)Enum.Parse(typeof(PaymentMethods), oMethod));

            builder.Property(o => o.Status).HasConversion(
        (oMethod) => oMethod.ToString(),
        (oMethod) => (OrderStatus)Enum.Parse(typeof(OrderStatus), oMethod));


        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSys.Core.Entities;


namespace OrderSys.Repository.Data.Configurations
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {

            builder.Property(i => i.TotalAmount)
                   .HasColumnType("decimal (18,2)");


            builder.HasOne(i => i.Order)
                   .WithOne()
                   .HasForeignKey<Invoice>(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

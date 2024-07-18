using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;

namespace OrderSys.Repository.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u=>u.Username).IsUnique();

            builder.Property(u => u.Role).HasConversion(
            (Urole) => Urole.ToString(),
            (Urole) => (UserRole)Enum.Parse(typeof(UserRole), Urole));
        }
    }
}

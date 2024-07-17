using OrderSys.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public IReadOnlyList<OrderCustomerDto> Orders { get; set; } =null!;
    }
}

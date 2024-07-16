using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Entities;

namespace Order_Management_System.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}

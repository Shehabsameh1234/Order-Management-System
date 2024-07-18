using OrderSys.Core.Entities.Enums;


namespace OrderSys.Core.Entities
{
    public class Order :BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public PaymentMethods PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}

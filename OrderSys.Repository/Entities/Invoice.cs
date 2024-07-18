
namespace OrderSys.Core.Entities
{
    public class Invoice  :BaseEntity
    {
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; } =DateTime.Now;
        public decimal TotalAmount { get; set; }
        public Order Order { get; set; } = null!;
    }
}

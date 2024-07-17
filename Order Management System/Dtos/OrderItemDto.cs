﻿using OrderSys.Core.Entities;

namespace Order_Management_System.Dtos
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } 
    }
}
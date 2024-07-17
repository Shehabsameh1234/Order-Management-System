using AutoMapper;
using Order_Management_System.Dtos;
using OrderSys.Core.Entities;

namespace Order_Management_System.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<Order, OrderCustomerDto>();

            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        }
    }
}

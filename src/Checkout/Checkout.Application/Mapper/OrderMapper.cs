namespace Checkout.Application.Mapper
{
    using AutoMapper;
    using Checkout.Application.Commands;
    using Checkout.Application.Responses;
    using Checkout.Core.Entities;

    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}

namespace Checkout.API.Mapper
{
    using AutoMapper;
    using Checkout.Application.Commands;
    using EventBusRabbitMQ.Events;

    public class CheckoutMapper : Profile
    {
        public CheckoutMapper()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }
    }
}

namespace Basket.API.Mapping
{
    using AutoMapper;
    using Basket.Core.Entities;
    using EventBusRabbitMQ.Events;

    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<Checkout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}

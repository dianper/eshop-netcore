namespace Basket.API.Mapper
{
    using AutoMapper;
    using Basket.Core.Entities;
    using EventBusRabbitMQ.Events;

    public class BasketMapper : Profile
    {
        public BasketMapper()
        {
            CreateMap<Checkout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}

namespace Checkout.Application.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Checkout.Application.Commands;
    using Checkout.Application.Responses;
    using Checkout.Core.Entities;
    using Checkout.Core.Repositories;
    using MediatR;

    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public CheckoutOrderHandler(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = this.mapper.Map<Order>(request);
            if (orderEntity == null)
            {
                throw new ApplicationException("not mapped");
            }

            var newOrder = await this.orderRepository.AddAsync(orderEntity);

            return this.mapper.Map<OrderResponse>(newOrder);
        }
    }
}

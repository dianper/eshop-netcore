namespace Checkout.Application.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Checkout.Application.Queries;
    using Checkout.Application.Responses;
    using Checkout.Core.Repositories;
    using MediatR;

    public class GetOrderByUserNameHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrderByUserNameHandler(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await this.orderRepository.GetOrdersByUserName(request.UserName);

            return this.mapper.Map<IEnumerable<OrderResponse>>(orders);
        }
    }
}

namespace Checkout.Application.Queries
{
    using System;
    using System.Collections.Generic;
    using Checkout.Application.Responses;
    using MediatR;

    public class GetOrderByUserNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }

        public GetOrderByUserNameQuery(string userName)
        {
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Infrastructure.Data
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}

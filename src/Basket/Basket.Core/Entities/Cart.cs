﻿namespace Basket.Core.Entities
{
    using System.Collections.Generic;

    public class Cart
    {
        public Cart() { }

        public Cart(string username)
        {
            this.UserName = username;
        }

        public string UserName { get; set; }

        public IEnumerable<CartItems> Items { get; set; } = new List<CartItems>();

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;

                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }

                return totalPrice;
            }
        }
    }
}

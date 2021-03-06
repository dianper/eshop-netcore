﻿namespace Checkout.Application.Responses
{
    using System;

    public class OrderResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public decimal TotalPrice { get; set; }

        // Billing Address
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string Expiration { get; set; }

        public string CVV { get; set; }

        public int PaymentMethod { get; set; }
    }
}

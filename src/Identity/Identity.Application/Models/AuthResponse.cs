namespace Identity.Application.Models
{
    using System;

    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}

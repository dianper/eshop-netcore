namespace Identity.Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

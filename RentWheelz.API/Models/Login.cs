using System.ComponentModel.DataAnnotations;

namespace RentWheelz.API.Models
{
    public class Login
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
    }
}

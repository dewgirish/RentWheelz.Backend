using System.ComponentModel.DataAnnotations;

namespace RentWheelz.API.Models
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        public string ProofId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace RentWheelz.Data.Models
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

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
using System.ComponentModel.DataAnnotations;

namespace RentWheelz.Data.Models
{
    public class Reservation : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string BookingId { get; set; }
        public string UserEmail { get; set; }
        public string CarId { get; set; }
        public DateOnly ReservationDate { get; set; }
        public DateOnly PickupDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public int NumOfTravellers { get; set; }
        public string Status { get; set; }
        public string Car { get; set; }
        public string Img { get; set; }
        public double Total { get; set; }
    }
}
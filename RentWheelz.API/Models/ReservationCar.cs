namespace RentWheelz.API.Models
{
    public class ReservationCar
    {
        public DateOnly pickupDate { get; set; }
        public DateOnly returnDate { get; set; }
        public int numOfTravellers { get; set; }
        public string carId { get; set; }
    }
}
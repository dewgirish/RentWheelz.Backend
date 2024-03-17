namespace RentWheelz.API.Models
{
    public class AddCar
    {
        public string CarModel { get; set; }
        public string RegistrationNumber { get; set; }
        public bool CarAvailability { get; set; }
        public string Brand { get; set; }
        public int PricePerHour { get; set; }
        public string Thumbnail { get; set; }
    }
}
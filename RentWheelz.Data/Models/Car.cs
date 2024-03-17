namespace RentWheelz.Data.Models
{
    public class Car : IEntity
    {
        public int Id { get; set; }
        public string CarId { get; set; }
        public string CarModel { get; set; }
        public string RegistrationNumber { get; set; }
        public string CarAvailability { get; set; }
        public string Brand { get; set; }
        public int PricePerHour { get; set; }
        public string Thumbnail { get; set; }
    }
}
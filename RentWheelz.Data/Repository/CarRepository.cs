using RentWheelz.Data.Models;
using RentWheelz.Data.Repository.Generic;

namespace RentWheelz.Data.Repository
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(RentWheelzContext context) : base(context)
        {
        }
    }
}
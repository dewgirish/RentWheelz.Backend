using RentWheelz.Data.Models;
using RentWheelz.Data.Repository.Generic;

namespace RentWheelz.Data.Repository
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(RentWheelzContext context) : base(context)
        {
        }
    }
}
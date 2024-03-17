using RentWheelz.Data.Repository;

namespace RentWheelz.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User
        {
            get;
        }

        ICarRepository Car
        {
            get;
        }

        IReservationRepository Reservation
        {
            get;
        }

        int Save();
    }
}
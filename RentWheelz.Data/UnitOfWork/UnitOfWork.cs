using RentWheelz.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentWheelz.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private RentWheelzContext context;

        public UnitOfWork(RentWheelzContext context)
        {
            this.context = context;
            User = new UserRepository(this.context);
            Car = new CarRepository(this.context);
            Reservation = new ReservationRepository(this.context);
        }

        public IUserRepository User
        {
            get;
            private set;
        }

        public ICarRepository Car
        {
            get;
            private set;
        }

        public IReservationRepository Reservation
        {
            get;
            private set;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
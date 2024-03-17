using RentWheelz.Data.Models;
using RentWheelz.Data.Repository.Generic;

namespace RentWheelz.Data.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(RentWheelzContext context) : base(context)
        {
        }
    }
}
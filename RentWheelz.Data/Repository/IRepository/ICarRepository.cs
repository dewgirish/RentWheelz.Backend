using RentWheelz.Data.Models;
using RentWheelz.Data.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentWheelz.Data.Repository
{
    public interface ICarRepository : IGenericRepository<Car>
    {
    }
}
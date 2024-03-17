using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentWheelz.Data.Models;

namespace RentWheelz.Data
{
    public class RentWheelzContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public RentWheelzContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
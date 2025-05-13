using Microsoft.EntityFrameworkCore;

namespace BookingApp.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options): base(options) { }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}

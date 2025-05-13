using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Models
{
    [Table("Venue")]
    public class Venue
    {
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set;}

        public int Capacity { get; set; }

        public string? ImageURL { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
       
        public List<Booking> Bookings { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Models
{
    [Table("Event")]
    public class Event

    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public string Description { get; set; }

        public int VenueID  { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}

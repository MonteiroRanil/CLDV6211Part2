using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDBContext _context;

        public BookingController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Booking with search
        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.Venue.VenueName.Contains(searchString) ||
                    b.Event.EventName.Contains(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (await IsVenueBooked(booking.VenueID, booking.BookingDate))
            {
                ModelState.AddModelError("BookingDate", "Venue already booked for this date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    HandleDbUpdateException(ex);
                }
            }

            PopulateDropdowns(booking.EventID, booking.VenueID);
            return View(booking);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            return booking == null ? NotFound() : View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            PopulateDropdowns(booking.EventID, booking.VenueID);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID)) return NotFound();
                    throw;
                }
            }
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            return booking == null ? NotFound() : View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }

        private async Task<bool> IsVenueBooked(int venueId, DateTime date)
        {
            return await _context.Bookings
                .AnyAsync(b => b.VenueID == venueId &&
                              b.BookingDate.Date == date.Date);
        }

        private void PopulateDropdowns(object selectedEvent = null, object selectedVenue = null)
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "EventName", selectedEvent);
            ViewData["VenueID"] = new SelectList(_context.Venues, "VenueID", "VenueName", selectedVenue);
        }

        private void HandleDbUpdateException(DbUpdateException ex)
        {
            ModelState.AddModelError("", "An error occurred while saving.");
            if (ex.InnerException?.Message.Contains("unique constraint") == true)
            {
                ModelState.AddModelError("BookingDate", "Venue already booked for this date.");
            }
        }
    }
}
using System.Reflection;
using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EventController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Event = await _context.Events.ToListAsync();
            return View(Event);
        }
        public IActionResult Create()
        {

            ViewBag.VenueList = new SelectList(_context.Venues, "VenueID", "VenueName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event evnt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evnt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evnt);
        }
        public async Task<IActionResult> Details(int? id)
        {

            var evnt = await _context.Events.FirstOrDefaultAsync(m => m.EventID == id);

            if (evnt == null)
            {
                return NotFound();
            }
            return View(evnt);
        }
        private bool EventExists(int id)
        {
            return _context.Events.Any(m => m.EventID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evnt = await _context.Events.FindAsync(id);

            if (evnt == null)
            {
                return NotFound();
            }
            ViewBag.VenueList = new SelectList(_context.Venues, "VenueID", "VenueName", evnt.VenueID);

            return View(evnt);
        }
        [HttpPost]
           
            public async Task<IActionResult> Edit(int id, Event evnt)
            {
                if (id != evnt.EventID) 
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(evnt);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Events.Any(e => e.EventID == evnt.EventID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(evnt);
            }




        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evnt = await _context.Events.FirstOrDefaultAsync(m => m.EventID == id);

            if (evnt == null)
            {
                return NotFound();
            }

            return View(evnt);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null) return NotFound();

            var isBooked = await _context.Bookings.AnyAsync(b => b.EventID == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }
            
                _context.Events.Remove(evnt);
                await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        //error handling
            
        }

    }


            


 
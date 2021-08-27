using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMDM.Data;
using AMDM.Models;
using Microsoft.AspNetCore.Http;
using AMDM.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace AMDM.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly AMDMContext _context;
        private readonly TicketService _service;

        public TicketsController(AMDMContext context, TicketService service)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Search(string query, string traineeId, string purchaseDateFilter, string expiredDateFilter)
        {
            /*var*/
            IQueryable<Ticket> aMDMContext = _context.Ticket
                .Include(t => t.TicketType)
                .Include(t => t.Trainee)
                .Where(t => (query == null
                        || t.Trainee.FirstName.Contains(query)
                        || t.Trainee.LastName.Contains(query)
                        || t.Trainee.Id.Contains(query)
                        || t.TicketType.Name.Contains(query)));

            if (traineeId != null)
            {
                aMDMContext = aMDMContext.Where(t => t.Trainee.Id.Equals(traineeId));
            }

            if (purchaseDateFilter == "today")
            {
                aMDMContext = aMDMContext.Where(ticket => ticket.PurchaseDate.CompareTo(DateTime.Now.Date)==0);
            }
            if (purchaseDateFilter == "yesterday")
            {
                aMDMContext = aMDMContext.Where(ticket => ticket.PurchaseDate.AddDays(1).Date.CompareTo(DateTime.Now.Date)==0);
            }
            if (purchaseDateFilter == "weekAgo")
            {
                aMDMContext = aMDMContext.Where(ticket => (ticket.PurchaseDate.CompareTo(DateTime.Now.Date) <=0  && ticket.PurchaseDate.CompareTo(DateTime.Now.Date.AddDays(-7)) >=0 ));
            }


            if (expiredDateFilter == "today")
            {
                aMDMContext = aMDMContext.Where(ticket => ticket.ExpiredDate.CompareTo(DateTime.Now)==0);
            }
            if (expiredDateFilter == "tomorrow")
            {
                aMDMContext = aMDMContext.Where(ticket => ticket.ExpiredDate.CompareTo(DateTime.Now.Date.AddDays(1))==0);
            }
            if (expiredDateFilter == "week")
            {
                aMDMContext = aMDMContext.Where(ticket => (ticket.ExpiredDate.CompareTo(DateTime.Now.Date) >=0 && ticket.ExpiredDate.CompareTo(DateTime.Now.Date.AddDays(7)) <=0 ));
            }

            var q = from t in aMDMContext
                        //orderby t.Date 
                    select new NewRecord(t.TicketType.Name, t.Id, t.Trainee.FirstName, t.Trainee.Id, t.RemainingPunchingHoles, t.PurchaseDate, t.ExpiredDate);


            //return View("Index", await aMDMContext.ToListAsync()); //NOT WORK

            return Json(await q.ToListAsync());

        }

        // GET: Tickets
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var aMDMContext = _context.Ticket.Include(t => t.TicketType).Include(t => t.Trainee);
            return View(await aMDMContext.ToListAsync());
        }


        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,Trainee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.TicketType)
                .Include(t => t.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,Trainee")]
        public async Task<IActionResult> DetailsByTraineeId(string? traineeId)
        {
            if (traineeId == null)
            {
                return NotFound();
            }

            var ticket = _context.Ticket
                .Include(t => t.TicketType)
                .Include(t => t.Trainee)
                .Where(m => m.Trainee.Id == traineeId);

            if (ticket == null)
            {
                return NotFound();
            }

            var q = from t in ticket
                        //orderby t.Date 
                    select new { t.TicketType.Name, t.TicketType.PunchingHolesNumber, t.RemainingPunchingHoles, t.ExpiredDate };
            return Json(await q.FirstOrDefaultAsync());
            //return Json(json);

        }

        // GET: Tickets/Create
        [Authorize(Roles = "Trainee")]
        public IActionResult Create()
        {
            ViewData["TicketTypeId"] = new SelectList(_context.TicketType, "Id", "Id");
            ViewData["TraineeId"] = new SelectList(_context.Trainee, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketTypeId,TraineeId,RemainingPunchingHoles,PurchaseDate,ExpiredDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketType, "Id", "Id", ticket.TicketTypeId);
            ViewData["TraineeId"] = new SelectList(_context.Trainee, "Id", "Id", ticket.TraineeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["TicketTypeId"] = new SelectList(_context.TicketType, "Id", "Id", ticket.TicketTypeId);
            ViewData["TraineeId"] = new SelectList(_context.Trainee, "Id", "Id", ticket.TraineeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketTypeId,TraineeId,RemainingPunchingHoles,PurchaseDate,ExpiredDate")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["TicketTypeId"] = new SelectList(_context.TicketType, "Id", "Id", ticket.TicketTypeId);
            ViewData["TraineeId"] = new SelectList(_context.Trainee, "Id", "Id", ticket.TraineeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.TicketType)
                .Include(t => t.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }

    internal record NewRecord(string Name, int Id, string FirstName, string Item, int RemainingPunchingHoles, DateTime PurchaseDate, DateTime ExpiredDate);
}

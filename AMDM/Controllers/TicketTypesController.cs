using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMDM.Data;
using AMDM.Models;
using AMDM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AMDM.Controllers
{
    [Authorize]
    public class TicketTypesController : Controller
    {
        private readonly AMDMContext _context;
        private readonly TicketTypeService _service;
        public TicketTypesController(AMDMContext context, TicketTypeService service)
        {
            _context = context;
            _service = service;
        }

        // GET: TicketTypes
        public async Task<IActionResult> Search(string query)
        {
            /*var*/
            IQueryable<TicketType> aMDMContext = _context.TicketType
                .Where(t => query == null
                        || t.Name.Contains(query)
                        || t.Price.ToString().Contains(query)
                        || t.PunchingHolesNumber.ToString().Contains(query));

            var q = from t in aMDMContext
                    select new { t.Name, t.Price, t.TicketPeriod, t.PunchingHolesNumber };


            return Json(await q.ToListAsync());

        }
        [Authorize(Roles = "Admin,Trainee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketType.ToListAsync());
        }


        // GET: TicketTypes/Details/5
        [Authorize(Roles = "Admin,Trainee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }
        // GET: TrainingTypes/Purchase
        //public IActionResult Purchase()
        //{
        //    return View();
        //}

        // POST: TrainingTypes/Purchase
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int ticketTypeId)
        {
            if (ModelState.IsValid)
            {
                var traineeId = HttpContext.Session.GetString("Id");
                try
                {
                    await Task.Run(() => _service.Purchase(ticketTypeId, traineeId));
                    return Ok();
                }
                catch(Exception e)
                {
                    return BadRequest(new { Error = e.Message });
                }
                
                //return View(training);
                //return View(await _context.Training.Include(Trainee).)?????
            }
            return BadRequest(new { Error = "model is not valid" });
        }


        // GET: TicketTypes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,TicketPeriod,PunchingHolesNumber")] TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketType);
        }

        // GET: TicketTypes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketType.FindAsync(id);
            if (ticketType == null)
            {
                return NotFound();
            }
            return View(ticketType);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,TicketPeriod,PunchingHolesNumber")] TicketType ticketType)
        {
            if (id != ticketType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTypeExists(ticketType.Id))
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
            return View(ticketType);
        }
        [Authorize(Roles = "Admin")]
        // GET: TicketTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketType = await _context.TicketType.FindAsync(id);
            _context.TicketType.Remove(ticketType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TicketTypeExists(int id)
        {
            return _context.TicketType.Any(e => e.Id == id);
        }
    }
}

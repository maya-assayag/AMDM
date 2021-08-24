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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using AMDM.Services;

namespace AMDM.Controllers
{
    public class TraineesController : Controller
    {
        private readonly AMDMContext _context;
        private readonly UserService _service;

        public TraineesController(AMDMContext context, UserService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Trainees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainee.ToListAsync());
        }
        public async Task<IActionResult> Search(string query/*, string dateFilter*/)
        {
            /*var*/
            IQueryable<Trainee> aMDMContext = _context.Trainee
                .Where(t =>
                        query == null
                        || t.FirstName.Contains(query)
                        || t.LastName.Contains(query)
                        || t.PhoneNumber.ToString().Contains(query)
                        || t.Id.ToString().Contains(query)
                        || t.Email.Contains(query));

            //LinQ:
            //Example
            //var q = from a in _context.Training.Include(t => t.Trainer).Include(t => t.TrainingType)
            //        where (query == null
            //            || a.Trainer.FirstName.Contains(query)
            //            || a.Trainer.LastName.Contains(query)
            //            || a.TrainingType.Name.Contains(query))
            //        join ...
            //        group ...
            //        orderby a.Date descending
            //        select a;//return each one from the resulte that predicated true on the filter  
            // or
            //        select a.TrainingType.Name
            // or you can make an aninomys object and return him
            //        select new { Id= a.Id , Summary = a.TrainingType.Name ....};
            //
            //filter example:
            //if (dateFilter == "today")
            //{
            //    aMDMContext.Where(training => training.Date == DateTime.Now.Date);
            //}
            //if (dateFilter == "tomorrow")
            //{
            //    aMDMContext.Where(training => training.Date == DateTime.Now.Date.AddDays(1));
            //}
            //if (dateFilter == "week")
            //{
            //    aMDMContext.Where(training => (training.Date >= DateTime.Now.Date && training.Date <= DateTime.Now.Date.AddDays(7)));
            //}

            var q = from t in aMDMContext
                        //orderby t.Date 
                    select new { t.Id, t.FirstName, t.LastName, t.Email, t.PhoneNumber };


            //return View("Index", await aMDMContext.ToListAsync()); //NOT WORK

            return Json(await q.ToListAsync());

        }

        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainee trainee = await _context.Trainee
                .Include(trainee => trainee.Ticket)
                .Include(trainee => trainee.Trainings).ThenInclude(training => training.TrainingType)
                .Include(trainee => trainee.Trainings).ThenInclude(training => training.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // GET: Trainees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Height,Weight,Email,Password,PhoneNumber,TraineeGender")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                if ((trainee.DateOfBirth > DateTime.Now.Date.AddYears(-10)) || (trainee.DateOfBirth < DateTime.Now.Date.AddYears(-120)))
                {
                    ViewData["Error"] = "Registration failed, the studio allow to 10-120 ages, please try again";
                }
                else
                {
                   var t = _context.Trainee.FirstOrDefault(t =>
                   t.Id==trainee.Id);
                   var t2 = _context.Trainee.FirstOrDefault(t =>
                   t.Email.Equals(trainee.Email));
                    if (t == null && t2==null)
                    {
                        _context.Add(trainee);
                        //trainee.Ticket = new Ticket();
                        await _context.SaveChangesAsync();
                        User u = new User();
                        u.Email = trainee.Email;
                        u.Password = trainee.Password;
                        u.Type = UserType.Trainee;

                        bool res = await _service.Register(u, HttpContext);

                        if (res == true)
                        {
                            return RedirectToAction("TraineeIndex", "Home");
                        }
                        else
                        {
                            ViewData["Error"] = "Registration failed, please try again";
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "This user already exists in the system";
                    }
                }
            }
            else
            {
                ViewData["Error"] = "!!!!This user already exists in the system";
                return RedirectToAction("Login", "User");
            }
            return View(trainee);
        }

        // GET: Trainees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainee.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,DateOfBirth,Height,Weight,Email,Password,PhoneNumber,TraineeGender")] Trainee trainee)
        {
            if (id != trainee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeExists(trainee.Id))
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
            return View(trainee);
        }

        // GET: Trainees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainee = await _context.Trainee.FindAsync(id);
            _context.Trainee.Remove(trainee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(string id)
        {
            return _context.Trainee.Any(e => e.Id == id);
        }
    }
}

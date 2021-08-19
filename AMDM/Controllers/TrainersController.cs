using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMDM.Data;
using AMDM.Models;
using Microsoft.AspNetCore.Authorization;
using AMDM.Services;

namespace AMDM.Controllers
{
    [Authorize(Roles ="Admin,Trainer")]

    public class TrainersController : Controller
    {
        private readonly AMDMContext _context;
        private readonly UserService _service;

        public TrainersController(AMDMContext context, UserService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainer.ToListAsync());
        }
        public async Task<IActionResult> Search(string query/*, string dateFilter*/)
        {
            /*var*/
            IQueryable<Trainer> aMDMContext = _context.Trainer
                .Where(t =>
                        query == null
                        || t.FirstName.Contains(query)
                        || t.LastName.Contains(query)
                        || t.Email.Contains(query)
                        || t.PhoneNumber.ToString().Contains(query)
                        || t.Id.ToString().Contains(query));

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
                    select new { t.Id, t.FirstName, t.LastName };


            //return View("Index", await aMDMContext.ToListAsync()); //NOT WORK

            return Json(await q.ToListAsync());

        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Height,Weight,Email,Password,PhoneNumber,TrainerGender")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = trainer.Email;
                user.Password = trainer.Password;
                user.Type = UserType.Trainer;

                var res= await _service.Register(user, HttpContext);
                if(res==true)
                {
                    _context.Add(trainer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                
                    ViewData["Error"] = "This user/trainer already exists in the system";

                    return View(trainer);
                }
                
                
            }
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,DateOfBirth,Height,Weight,Email,Password,PhoneNumber,TrainerGender")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trainer = await _context.Trainer.FindAsync(id);
            _context.Trainer.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(string id)
        {
            return _context.Trainer.Any(e => e.Id == id);
        }
    }
}

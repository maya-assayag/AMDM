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
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainer.ToListAsync());
        }
        public async Task<IActionResult> GetAllTrainersNames()
        {
            var allTrainersNames =_context.Trainer;
            var q = from t in allTrainersNames
                        //orderby t.Date 
                    select new { t.FirstName };

            return Json(await q.ToListAsync());
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
                        || t.PhoneNumber.Contains(query)
                        || t.Id.Contains(query));

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
                    select new { t.FirstName, t.LastName, t.Id, t.DateOfBirth, t.Height, t.Weight, t.Email, t.Password, t.PhoneNumber, t.TrainerGender };


            //return View("Index", await aMDMContext.ToListAsync()); //NOT WORK

           return Json(await q.ToListAsync());

        }

        // GET: Trainers/Details/5
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer trainer = await _context.Trainer
                .Include(trainee => trainee.Trainings)
                .ThenInclude(training => training.TrainingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        [Authorize(Roles = "Admin")]
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
                if ((trainer.DateOfBirth > DateTime.Now.Date.AddYears(-16)) || (trainer.DateOfBirth < DateTime.Now.Date.AddYears(-70)))
                {
                    ViewData["Error"] = "failed to create trainer, trainer must be 16-70 years old";
                }
                else
                {
                    var t = _context.Trainer.FirstOrDefault(t =>t.Id == trainer.Id);
                    var t2 = _context.User.FirstOrDefault(t =>t.Email.Equals(trainer.Email));
                    var t3 = _context.Trainee.FirstOrDefault(t => t.Id == trainer.Id);


                    if (t == null && t2 == null && t3==null)
                    {
                        User user = new User();
                        user.Email = trainer.Email;
                        user.Password = trainer.Password;
                        user.Type = UserType.Trainer;

                        var res = await _service.Register(user, HttpContext);
                        if (res == true)
                        {
                            _context.Add(trainer);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ViewData["Error"] = "This user/trainer already exists in the system";
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "This user/trainer already exists in the system";
                    }

                }

            }
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        [Authorize(Roles = "Admin,Trainer")]
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
                if ((trainer.DateOfBirth > DateTime.Now.Date.AddYears(-16)) || (trainer.DateOfBirth < DateTime.Now.Date.AddYears(-70)))
                {
                    ViewData["Error"] = "failed to create trainer, trainer must be 16-70 years old";
                }
                else
                {
                    var t = _context.Trainer.FirstOrDefault(t => t.Email.Equals(trainer.Email) && !(t.Id.Equals(id)));
                    var t2 = _context.Trainee.FirstOrDefault(t => t.Email.Equals(trainer.Email));

                    Trainer trainerForUser = _context.Trainer.FirstOrDefault(t => t.Id.Equals(id));
                    User user = _context.User.FirstOrDefault(user => user.Email.Equals(trainerForUser.Email));

                    if (t == null && t2==null && user !=null)
                    {
                        try
                        {
                            _context.Remove(user);
                            User u = new User();
                            u.Email = trainer.Email;
                            u.Password = trainer.Password;
                            u.Type = UserType.Trainer;
                            try
                            {
                                _context.Update(trainer);
                            }
                            catch(Exception)
                            {

                            }
                            
                            _context.Add(u);
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
                        return RedirectToAction("Details", new { id = trainer.Id });
                    }
                    else
                    {
                        ViewData["Error"] = "This email address is already exists in the system";
                    }
                }
            }
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        [Authorize(Roles = "Admin")]
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
            Trainer trainer = await _context.Trainer
               .Include(trainee => trainee.Trainings)
               .ThenInclude(training => training.TrainingType)
               .FirstOrDefaultAsync(m => m.Id == id);
            for (int i = 0; i < trainer.Trainings.Count; i++)
            {
                if (trainer.Trainings[i].Date > DateTime.Now.Date || (trainer.Trainings[i].Date == DateTime.Now.Date && trainer.Trainings[i].Time >= DateTime.Now))
                {
                    ViewData["Error"] = "You can't delete trainer who have future trainings";
                    return View(trainer);
                }
            }
            var user = await _context.User.FindAsync(trainer.Email);
            _context.Trainer.Remove(trainer);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(string id)
        {
            return _context.Trainer.Any(e => e.Id == id);
        }
    }
}

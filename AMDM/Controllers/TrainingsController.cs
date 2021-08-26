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

namespace AMDM.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly AMDMContext _context;
        private readonly TrainingService _service;

        public TrainingsController(AMDMContext context, TrainingService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            var aMDMContext = _context.Training
                .Include(t => t.Trainer)
                .Include(t => t.Trainees)
                .Include(t => t.TrainingType)
                .OrderBy(t => t.Date);

            
            return View(await aMDMContext.ToListAsync());
        }
        public async Task<IActionResult> Search(string query, string dateFilter, string typeFilter, string trainerFilter)
        {
            /*var*/
            IQueryable<Training> aMDMContext = _context.Training
                .Include(t => t.Trainer)
                .Include(t => t.TrainingType)
                .Include(t=> t.Trainees)
                .Where(t=>
                        t.Date>= DateTime.Now.Date
                        && (query == null
                        || t.Trainer.FirstName.Contains(query) 
                        || t.Trainer.LastName.Contains(query) 
                        || t.TrainingType.Name.Contains(query)));
            if (typeFilter != null && !typeFilter.Equals("all"))
            {
                aMDMContext = aMDMContext.Where(t => t.TrainingType.Name.Equals(typeFilter));
            }
            if (trainerFilter != null && !trainerFilter.Equals("all"))
            {
                aMDMContext = aMDMContext.Where(t => t.Trainer.FirstName.Equals(trainerFilter));
            }

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
            if (dateFilter == "today")
            {
                aMDMContext = aMDMContext.Where(training => DateTime.Compare(training.Date,DateTime.Now.Date)==0);
            }
            if (dateFilter == "tomorrow")
            {
                aMDMContext = aMDMContext.Where(training => DateTime.Compare(training.Date, DateTime.Now.Date.AddDays(1)) == 0);
            }
            if (dateFilter == "week")
            {
                aMDMContext = aMDMContext.Where(training => (DateTime.Compare(training.Date,DateTime.Now.Date)>=0 && DateTime.Compare(training.Date,DateTime.Now.Date.AddDays(7))<=0));
            }

            var q = from t in aMDMContext
                        //orderby t.Date 
                    select new { t.TrainingType.Name, t.Trainer.FirstName, t.Date, t.Time, t.Studio, t.MaxRegisterTrainees, t.TotalTraineesLeft };


            //return View("Index", await aMDMContext.ToListAsync()); //NOT WORK

            return Json(await q.ToListAsync());
            
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Trainer)
                .Include(t => t.TrainingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        public IActionResult Create()
        {
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id");
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name");
            //var training = _context.Training.Include(training => training.Trainer);
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrainingTypeId,Date,Time,Studio,MaxRegisterTrainees")] Training training)
        {
            training.TrainerId = HttpContext.Session.GetString("Id");
            if (ModelState.IsValid)
            {
                if(training.Date<DateTime.Now.Date ||(training.Date==DateTime.Now.Date && training.Time<DateTime.Now))
                {
                    ViewData["Error"] = "Failed to create training, The date and time of the training can't be in the past!, please try again";
                }
                else
                {
                    _context.Add(training);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id", training.TrainerId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name", training.TrainingTypeId);
            return View(training);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id", training.TrainerId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name", training.TrainingTypeId);
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingTypeId,TrainerId,Date,Time,Studio,MaxRegisterTrainees")] Training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
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
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id", training.TrainerId);
            ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name", training.TrainingTypeId);
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Trainer)
                .Include(t => t.TrainingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Training.FindAsync(id);
            _context.Training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.Id == id);
        }

        // GET: Trainings/Register
        //public IActionResult Register()
        //{
        //ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id");
        //ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name");
        //    return View();
        //}

        // POST: Trainings/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpGet]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int trainingID)
        {
            
            if (ModelState.IsValid)
            {
                var Id = HttpContext.Session.GetString("Id");
                try
                {
                    await Task.Run(() => _service.Register(trainingID, Id));
                    return Ok();
                }
                catch (Exception e)
                {
                 
                     ViewData["Error"] = e.Message;

                    return BadRequest(new { Error = e.Message });




                }
                
                //return View(training);
                //return View(await _context.Training.Include(Trainee).)?????
            }
            return BadRequest(new { Error = "model is not valid" });


            //ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id", training.TrainerId);
            //ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name", training.TrainingTypeId);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(int trainingID)
        {
            if (ModelState.IsValid)
            {
                var Id = HttpContext.Session.GetString("Id");
                await Task.Run(() => _service.Unregister(trainingID, Id));
                return Ok();
                //return View(training);
                //return View(await _context.Training.Include(Trainee).)?????
            }
            return BadRequest(new { Error = "model is not valid" });

            //ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "Id", training.TrainerId);
            //ViewData["TrainingTypeId"] = new SelectList(_context.TrainingType, "Id", "Name", training.TrainingTypeId);

        }
    }
}

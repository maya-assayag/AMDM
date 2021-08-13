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
            var aMDMContext = _context.Training.Include(t => t.Trainer).Include(t => t.Trainees).Include(t => t.TrainingType);
            
            return View(await aMDMContext.ToListAsync());
        }
        public async Task<IActionResult> Search(string query)
        {
            var aMDMContext = _context.Training.Include(t => t.Trainer).Include(t => t.TrainingType)
                .Where(t=>
                        t.Trainer.FirstName.Contains(query) 
                        || t.Trainer.LastName.Contains(query) 
                        || t.TrainingType.Name.Contains(query)
                        || query==null);

            var q = from t in aMDMContext
                    select new { t.Id, t.Studio, t.Trainer, t.TrainingType.Name };

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
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrainingTypeId,TrainerId,Date,Time,Studio,MaxRegisterTrainees")] Training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                await Task.Run(() => _service.Register(trainingID, Id));
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

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
       
        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(string id)
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
                var t = _context.Trainee.FirstOrDefault(t =>
                   t.Id == trainee.Id);
                if (t == null)
                {
                    _context.Add(trainee);
                    await _context.SaveChangesAsync();
                    User u = new User();
                    u.Email = trainee.Email;
                    u.Password = trainee.Password;
                    u.Type = UserType.Trainee;

                    bool res = await _service.Register(u, HttpContext);

                    if (res == true)
                    {
                        return RedirectToAction(nameof(Index), "Home");
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

                return View(trainee);
            }
            else
            {
                ViewData["Error"] = "!!!!This user already exists in the system";
                return RedirectToAction("Login", "User");
            }
            //return View(trainee);
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

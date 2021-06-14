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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AMDM.Services;

namespace AMDM.Controllers
{
    public class UsersController : Controller
    {
        private readonly AMDMContext _context;
        private readonly UserService _service;

        public UsersController(AMDMContext context, UserService service)
        {
            _context = context;
            _service = service;
        }


        public async void Register(User user)
        {
            var q = _context.User.FirstOrDefault(u => u.Email == user.Email);
            if (q == null)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();



                var u = _context.User.FirstOrDefault(u =>
                     u.Email == user.Email && u.Password == user.Password);


                Signin(u);

            }
            else
            {
                ViewData["Error"] = "This user already exists.";
            }
            
        }



        public async Task<IActionResult>Logout()
        {
            //HttpContext.Sessiom.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //// GET: Users/Register
        //public IActionResult Register()
        //{
        //    return View();
        //}
        // GET: Users/Type
        public IActionResult Type()
        {
            return View();
        }
        // POST: Users/Type
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Type([Bind("Type")] User user)
        {
            //if (ModelState.IsValid)
            //{
            //    if (user.Type.ToString() == "Trainee")
            //    {
            //        return RedirectToAction("Create", "Trainees");
            //    }

            //}
            if (user.Type.ToString() == "Trainee")
            {
                return RedirectToAction("Create", "Trainees");
            }
            if (user.Type.ToString() == "Trainer")
            {
                return RedirectToAction("Create", "Trainers");
            }
            return View(user);
        }







        ////***************************************************************************************
        //// POST: Users/Register
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register([Bind("Email,Password,Type")] User user)
        //{
        //   // if (ModelState.IsValid)
        //   // {

        //        var q = _context.User.FirstOrDefault(u => u.Email == user.Email);
        //        if (q == null)
        //        {
        //            _context.Add(user);
        //            await _context.SaveChangesAsync();



        //            var u = _context.User.FirstOrDefault(u =>
        //                 u.Email == user.Email && u.Password == user.Password);
        //            //if(user.Type.ToString() == "Trainee")
        //            //{
        //            //    RedirectToAction("Create", "Trainees");
        //            //}


        //            Signin(u);


        //            return RedirectToAction(nameof(Index), "Home");
        //        }
        //        else
        //        {
        //            ViewData["Error"] = "This user already exists.";
        //        }
        //    //}
        //    return View(user);
        //}
//***************************************************










        //// POST: Users/Register
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register([Bind("Email,Password")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var q = _context.User.FirstOrDefault(u => u.Email == user.Email);
        //        if (q == null)
        //        {
        //            _context.Add(user);
        //            await _context.SaveChangesAsync();



        //            var u = _context.User.FirstOrDefault(u =>
        //                 u.Email == user.Email && u.Password == user.Password);
        //            //if(user.Type.ToString() == "Trainee")
        //            //{
        //            //    RedirectToAction("Create", "Trainees");
        //            //}


        //            Signin(u);


        //            return RedirectToAction(nameof(Index), "Home");
        //        }
        //        else
        //        {
        //            ViewData["Error"] = "This user already exists.";
        //        }
        //    }
        //    return View(user);
        //}




        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }
        private async void Signin(User account)
        {
            var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
             //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
                ,authProperties);
        }


                // POST: Users/Login
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => 
                    u.Email == user.Email && u.Password==user.Password);
                if(q!=null)
                {
                    if (q.Type == UserType.Trainee)
                    {
                        Trainee trainee = _context.Trainee.FirstOrDefault(t =>
                            t.Email == user.Email);
                        if (q != null)
                        {
                            _service.Signin(q, HttpContext);
                            return RedirectToAction(nameof(Index), "Home", trainee);
                        }
                        else
                        {
                            ViewData["Error"] = "Email and/or password are incorrect";
                        }

                    }
                    else if (q.Type == UserType.Trainer)
                    {
                        Trainer trainer = _context.Trainer.FirstOrDefault(t =>
                            t.Email == user.Email);
                        if (q != null)
                        {
                            _service.Signin(q, HttpContext);
                            return RedirectToAction(nameof(Index), "Home", trainer);
                        }
                        else
                        {
                            ViewData["Error"] = "Email and/or password are incorrect";
                        }
                    }
                    else
                    {
                        if (q != null)
                        {
                            _service.Signin(q, HttpContext);
                            return RedirectToAction("AdminIndex", "Home");
                        }
                        else
                        {
                            ViewData["Error"] = "Email and/or password are incorrect";
                        }

                    }
                }
                
               
            }
            return View(user);
        }



        // GET: Users/Login
        public IActionResult AccessDenied()
        {
            return View();
        }



        //// GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.User.ToListAsync());
        //}

        //// GET: Users/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.Email == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Email,Password,Type")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Email,Password,Type")] User user)
        //{
        //    if (id != user.Email)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Email))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.Email == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var user = await _context.User.FindAsync(id);
        //    _context.User.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(string id)
        //{
        //    return _context.User.Any(e => e.Email == id);
        //}
    }
}

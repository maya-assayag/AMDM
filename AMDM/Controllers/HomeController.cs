using AMDM.Data;
using AMDM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AMDMContext _context;
        private readonly ILogger<HomeController> _logger;
        //private readonly MoreDemosContext _context;

        public HomeController(ILogger<HomeController> logger, AMDMContext context)
        {
            _context = context;
            _logger = logger;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Index(/*Trainee trainee*/)
        {
            var traineeId = HttpContext.Session.GetString("Id");
            Trainee trainee_ = _context.Trainee
                .Include(t => t.Ticket)
                .Include(t =>t.Trainings)
                .FirstOrDefault(t =>
                              t.Id == traineeId/*trainee.Id*/);
            return View(trainee_);
        }
        public IActionResult AdminIndex()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            //if(HttpContext.Session.GetString("email") == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

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
using TweetSharp;
using System.IO;
using System.Web;
using AppTwitter.Models;

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
        public IActionResult Index()
        {
            return View();
        }
         [HttpPost]
        public ActionResult Index(Tweets twts)//, HttpPostedFileBase postedFile)
        {
            string key = "fd65RkMwjagLVroErFwxnVRXa";
            string secret = "lk2TW0vcL0cdIjRGuIgIpnyvYxvHtdVXe5cMjJIW3queTpYtdp";
            string token = "1425442820232519684-2pb5cUcEovoAJjHKcIJi1mm7IytE9Z";
            string tokenSecret = "nwo3NWbsD9Qu3xQleYckrdormITF1jcp6Coq3ZhhHGiIg";

            string message = twts.tweets;

            //Enter the Image Path if you want to upload image .

             string  imagePath = @"C:\maorTest.jpg";/////////////////

            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);

            //this Condition  will check weather you want to upload a image & text or only text 
            if (imagePath.Length > 0)
            {
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    var result = service.SendTweetWithMedia(new SendTweetWithMediaOptions
                    {
                        Status = message,
                        Images = new Dictionary<string, Stream> { { "john", stream } }
                    });
                }
            }
            else // just message
            {
                var result = service.SendTweet(new SendTweetOptions
                {
                    Status = message
                });

            }

            twts.tweets = "";
            return View();
        }
        public IActionResult TraineeIndex()
        {
            var traineeId = HttpContext.Session.GetString("Id");
            Trainee trainee = _context.Trainee
                .Include(trainee => trainee.Ticket)
                .Include(trainee=> trainee.Trainings).ThenInclude(training=> training.TrainingType)
                .Include(trainee => trainee.Trainings).ThenInclude(training => training.Trainer)
                .FirstOrDefault(t =>
                              t.Id == traineeId);
            return View(trainee);
        }
        public IActionResult TrainerIndex()
        {
            var trainerId = HttpContext.Session.GetString("Id");
            Trainer trainer = _context.Trainer
                .Include(t => t.Trainings)
                .FirstOrDefault(t =>
                              t.Id == trainerId);
            return View(trainer);
        }
        public IActionResult AdminIndex()
        {
            AdminViewModel adminView = new AdminViewModel();
            var tickets = _context.Ticket
                .Include(t => t.TicketType)
                .ToList()
                .GroupBy(t => t.TicketType.Name);


            IList<int> counters = new List<int>();
            foreach (var ticket in tickets)
            {
                counters.Add(ticket.Count());
            }

            adminView.TicketsPie = counters;

            //TODO: get user data and insert into model
            // adminView.User = currentUser;

            return View(adminView);
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

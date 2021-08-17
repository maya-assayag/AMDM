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

        public IActionResult LandingPage()
        {
            return View();
        }
        public IActionResult Twitter()
        {
            Tweets twts = new Tweets();
            string key = "oxI5o7yTTILkj5pMrY5SssNc0";
            string secret = "EsB7x3NsHEf0TSApnhk0BLiUSBrcbfg9mMtr1HiFDaNoDdwS9O";
            string token = "1426478447614992387-ot1KHO0OWfSLK4tcPFqa7AlF1vFf3v";
            string tokenSecret = "6n8ciXQYIpRUJwJ3vO1Le0JxODVgGnLZvKmJ2HUIwT8V0";

            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);
            twts.AllTweets = (List<TwitterStatus>)service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
            {
                ScreenName = "AmdmGym",
            });

            return View(twts);
        }
        [HttpPost]
        public ActionResult Twitter(Tweets twts)
        {
            string key = "oxI5o7yTTILkj5pMrY5SssNc0";
            string secret = "EsB7x3NsHEf0TSApnhk0BLiUSBrcbfg9mMtr1HiFDaNoDdwS9O";
            string token = "1426478447614992387-ot1KHO0OWfSLK4tcPFqa7AlF1vFf3v";
            string tokenSecret = "6n8ciXQYIpRUJwJ3vO1Le0JxODVgGnLZvKmJ2HUIwT8V0";

            string message = twts.Tweet;
            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);
            twts.AllTweets = (List<TwitterStatus>)service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
            {
                ScreenName = "AmdmGym",
            });
            _context.SaveChangesAsync();


            service.SendTweet(new SendTweetOptions
            {
                Status = message
            });
            twts.Tweet = "";
            return View(twts);
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

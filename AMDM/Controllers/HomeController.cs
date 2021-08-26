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
using AppTwitter.Models;
using static AMDM.Models.AdminViewModel;
using System.Text.Json;

namespace AMDM.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly AMDMContext _context;
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, AMDMContext context)
        {
            _context = context;
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<IActionResult> LandingPage()
        {
            return View(await _context.Places.ToListAsync());
        }


        [Authorize(Roles = "Admin,Trainer")]
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
                ScreenName = "AmdmGym"
            });
            foreach(TwitterStatus tweet in twts.AllTweets)
            {
                tweet.CreatedDate=tweet.CreatedDate.AddHours(DateTimeOffset.Now.Offset.TotalHours);
            }
            return View(twts);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> GetAllTweets()
        {
            var allTweets = new Tweets();

            //var q = from t in allTypes
            //            //orderby t.Date 
            //        select new { t.Name };

            return Json(allTweets);
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

            service.SendTweet(new SendTweetOptions
            {
                Status = message
            });
            twts.AllTweets = (List<TwitterStatus>)service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
            {
                ScreenName = "AmdmGym"
            });
            foreach (TwitterStatus tweet in twts.AllTweets)
            {
                tweet.CreatedDate = tweet.CreatedDate.AddHours(DateTimeOffset.Now.Offset.TotalHours);
            }
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
                .ToList();

            var ticketsTypes=tickets
                .GroupBy(t => t.TicketType.Id);

            IList<KeyValuePair<string, int>> map = new List<KeyValuePair<string, int>>();


           
            foreach (var ticket in ticketsTypes)
            {
                map.Add(new KeyValuePair<string, int>(ticket.First().TicketType.Name + " with " + ticket.First().TicketType.PunchingHolesNumber + " punching", ticket.Count()));                    
            }


            adminView.TicketsTypesPurchasedLollipop = map;
            adminView.SumOfRevenueThisMonth = 0;

            foreach (var ticket in tickets)
            {
                if(ticket.PurchaseDate.Month==DateTime.Now.Month && ticket.PurchaseDate.Year == DateTime.Now.Year)
                {
                    adminView.SumOfRevenueThisMonth += ticket.TicketType.Price;
                }
                
            }

            adminView.SumOfTicketPurchasedThisMonth = 0;
            foreach (var ticket in tickets)
            {
                if (ticket.PurchaseDate.Month == DateTime.Now.Month && ticket.PurchaseDate.Year == DateTime.Now.Year)
                {
                    adminView.SumOfTicketPurchasedThisMonth ++;
                }

            }
            adminView.AllTrainees = _context.Trainee.Count();
            adminView.ActiveTrainees = _context.Trainee.Include(trainee => trainee.Ticket)
                .Where(trainee => (trainee.Ticket != null && trainee.Ticket.ExpiredDate > DateTime.Now))
                .ToList()
                .Count();

            var allTrainings = _context.Training
                .Include(training => training.Trainees)
                .Include(training => training.TrainingType)
                .Where(training => training.Date>=DateTime.Now.Date)
                .ToList();

            IList<BarplotItem> barplotItems = new List<BarplotItem>();


            foreach (Training training in allTrainings)
            {
                barplotItems.Add(new BarplotItem
                {

                    Id = training.Id,
                    Name = training.TrainingType.Name,
                    MaxParticipant = training.MaxRegisterTrainees,
                    ActualParticipant = training.MaxRegisterTrainees - training.TotalTraineesLeft
                });
            }

            adminView.AllTrainingsBarplot = barplotItems;


            //TODO: get user data and insert into model
            // adminView.User = currentUser;

            return View(adminView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

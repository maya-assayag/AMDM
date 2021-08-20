using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AMDM.Models;
using System.Net;
using System.Xml.Linq;

namespace AMDM.Controllers
{
    public class RSSFeedController : Controller
    {
        public IActionResult Index()
        {
            const string RSSURL = "https://rss.nytimes.com/services/xml/rss/nyt/Sports.xml";
            WebClient wclient = new WebClient();
            string RSSData = wclient.DownloadString(RSSURL);

            XDocument xml = XDocument.Parse(RSSData);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RSSFeed
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   PubDate = ((string)x.Element("pubDate"))
                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = RSSURL;
            return View();
        }
    }
}
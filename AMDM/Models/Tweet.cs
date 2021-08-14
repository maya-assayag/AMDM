using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TweetSharp;

namespace AppTwitter.Models
{
    public class Tweets
    {
        [Key] public int Id { get; set; }
        public string Tweet { get; set; }
        public List<TwitterStatus> AllTweets { get; set; }
    }
}
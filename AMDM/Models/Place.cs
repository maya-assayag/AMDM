using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Place
    {
        public int Id { get; set; }
        public String Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}

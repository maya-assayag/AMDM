using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Place
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z0-9, ]*$", ErrorMessage = "You must input a valid address begins with a capital letter")]
        public String Address { get; set; }
        [Required]
        [Range(-90, 90)]
        [Display(Name = "Latitude")]
        public double Lat { get; set; }
        [Required]
        [Range(-180, 180)]
        [Display(Name = "Longitude")]
        public double Lng { get; set; }
    }
}

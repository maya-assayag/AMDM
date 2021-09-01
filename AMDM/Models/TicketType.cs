using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class TicketType
    {
        public enum Period
        {
            Day=1,
            Week=7,
            Month=31,
            Year=365
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z ]*$", ErrorMessage = "You must begins with a capital letter, the allowed characters are english letters and spaces")]
        public string Name { get; set; }

        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        
        [Range(1, 10000)]
        [Required]
        [Display(Name = "Number of punching holes")]
        public int PunchingHolesNumber { get; set; }

        
        public  Period TicketPeriod { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}

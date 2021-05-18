using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class Ticket
    {
        [Key]
        public string Code { get; set; }
        [Display(Name = "Ticket type")]
        public TicketType TicketType { get; set; }

        public Trainee Trainee { get; set; }

        [Display(Name = "Remaining punching holes")]
        public int RemainingPunchingHoles { get; set; }

        [Display(Name = "Purchase date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDM.Models
{
    public class TicketType
    {
        [Key]
        public int Code { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public int PunchingHolesNumber { get; set; }
        public List<Ticket> Tickets { get; set; }



    }
}
